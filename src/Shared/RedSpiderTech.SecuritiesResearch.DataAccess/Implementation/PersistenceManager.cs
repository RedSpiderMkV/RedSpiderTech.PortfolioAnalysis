using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedSpiderTech.SecuritiesResearch.Common.Implementation.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Enums;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Implementation
{
    public class PersistenceManager : IPersistenceManager
    {
        #region Private Data

        private readonly IMySqlConnectionWrapper _connectionWrapper;
        private readonly IMySqlCommandWrapperFactory _commandWrapperFactory;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Public Methods

        public PersistenceManager(
            IMySqlConnectionWrapper connectionWrapper,
            IMySqlCommandWrapperFactory commandWrapperFactory,
            ILogger logger)
        {
            _logger = logger;
            _connectionWrapper = connectionWrapper;
            _commandWrapperFactory = commandWrapperFactory;

            _logger.Information("PersistenceManager: Opening database connection.");
            _connectionWrapper.Open();
            _logger.Information("PersistenceManager: Connection open.");
        }

        public bool DoesExchangeExist(IExchangeStaticDataModel exchangeStaticData)
        {
            _logger.Information($"PersistenceManager: Checking for the existence of exchange data: {exchangeStaticData.ExchangeName}");

            int? exchangeId = GetExchangeIdByName(exchangeStaticData.ExchangeName);

            string existenceLog = exchangeId.HasValue ? "exists" : "does not exist";
            _logger.Information($"PersistenceManager: Exchange {exchangeStaticData.ExchangeName} {existenceLog}");

            return exchangeId.HasValue;
        }

        public bool DoesSecurityStaticDataExist(ISecurityStaticDataModel securityStaticData)
        {
            _logger.Information($"PersistenceManager: Checking for the existence of security data: {securityStaticData.Symbol}");

            string commandString = "SELECT * FROM SecurityDetails WHERE Symbol = @Symbol;";
            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
            {
                commandWrapper.AddParameter("@Symbol", securityStaticData.Symbol);

                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        string existenceLog = dataReader.HasRows ? "exists" : "does not exist";
                        _logger.Information($"PersistenceManager: Security static data {securityStaticData.Symbol} {existenceLog}");

                        return dataReader.HasRows;
                    }
                }
            }
        }

        public void InsertExchange(IExchangeStaticDataModel exchangeStaticData)
        {
            _logger.Information($"PersistenceManager: Inserting Exchange: {exchangeStaticData}");

            string commandString = "INSERT INTO ExchangeDetails(Name) Values(@Name);";
            var parameters = new Dictionary<string, object> { { "@Name", exchangeStaticData.ExchangeName } };

            ExecuteNoReadQuery(commandString, 1, parameters);
        }

        public void InsertSecurityStatic(ISecurityStaticDataModel securityStaticData)
        {
            _logger.Information($"PersistenceManager: Inserting security static data: {securityStaticData.Symbol}");

            int? exchangeId = GetExchangeIdByName(securityStaticData.ExchangeData.ExchangeName);
            if (!exchangeId.HasValue)
            {
                throw new ArgumentException($"Attempting to insert static data {securityStaticData.ShortName} " +
                    $"for exchange {securityStaticData.ExchangeData.ExchangeName} which doesn't exist.");
            }

            string commandString = "INSERT INTO SecurityDetails(Name, Symbol, ExchangeId) Values(@Name, @Symbol, @ExchangeId);";
            var parameters = new Dictionary<string, object>
            {
                { "@Name", securityStaticData.ShortName},
                { "@Symbol", securityStaticData.Symbol},
                { "@ExchangeId", exchangeId.Value}
            };

            ExecuteNoReadQuery(commandString, 1, parameters);
        }

        public void InsertEndOfDayData(IEnumerable<IStockDataModel> stockDataCollection, VendorDetails vendorDetails)
        {
            IStockDataModel stockData = stockDataCollection.First();
            _logger.Information($"PersistenceManager: Inserting stock data for {stockData.Symbol}");

            int? securityId = GetSecurityIdBySymbol(stockData.Symbol);
            if (!securityId.HasValue)
            {
                throw new ArgumentException($"Attempting to insert stock data for {stockData.Symbol} which doesn't exist.");
            }

            var commandStringBuilder = new StringBuilder("INSERT INTO EndOfDayData(SecurityId, VendorId, Open, High, Low, Close, AdjustedClose, Volume, DayChange, DayPercentageChange, StandardChange, StandardPercentageChange, DateStamp) Values");

            foreach (IStockDataModel data in stockDataCollection)
            {
                string standardChange = data.StandardChange.HasValue ? data.StandardChange.ToString() : "NULL";
                string standardChangePercentage = data.StandardPercentageChange.HasValue ? data.StandardPercentageChange.ToString() : "NULL";
                commandStringBuilder.Append($"({securityId.Value}, {(int)vendorDetails}, {data.Open}, {data.High}, {data.Low}, {data.Close}, {data.AdjustedClose}, {data.Volume}, {data.DayChange}, {data.DayPercentageChange}, {standardChange}, {standardChangePercentage}, '{data.TimeStamp.Date.ToString("yyyy-MM-dd")}'),\n");
            }

            int rowCount = stockDataCollection.Count();
            _logger.Information($"PersistenceManager: Persisting {rowCount} rows for {stockData.Symbol}");

            commandStringBuilder.Length -= 2;
            ExecuteNoReadQuery(commandStringBuilder.ToString(), rowCount, new Dictionary<string, object>());
        }

        public DateTime GetLatestStockDateStamp(string symbol)
        {
            _logger.Information($"PersistenceManager: retrieving latest stock data timestamp for symbol: {symbol}");

            int? securityId = GetSecurityIdBySymbol(symbol);
            if (!securityId.HasValue)
            {
                throw new ArgumentException($"Security not found for symbol: {symbol}");
            }

            string commandString = "SELECT DateStamp FROM EndOfDayData WHERE SecurityId = @SecurityId ORDER BY DateStamp DESC LIMIT 1;";
            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
            {
                commandWrapper.AddParameter("@SecurityId", securityId.Value);

                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        if (!dataReader.HasRows || !dataReader.ReadNext())
                        {
                            return new DateTime(1950, 01, 01);
                        }

                        return dataReader.GetField<DateTime>("DateStamp");
                    }
                }
            }
        }

        public IEnumerable<ISecurityStaticDataModel> GetAllExistingStaticData()
        {
            _logger.Information($"PersistenceManager: retrieving All existing security static data.");

            string commandString = "select ed.Name ExchangeName, sd.Name SecurityName, sd.Symbol, sd.ExchangeId from SecurityDetails sd\n" +
                                    "join ExchangeDetails ed on ed.Id = sd.ExchangeId";

            var securityStaticDataCollection = new List<ISecurityStaticDataModel>();
            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
            {
                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        while (dataReader.ReadNext())
                        {
                            string exchangeName = dataReader.GetField<string>("ExchangeName");
                            var exchangeData = new ExchangeStaticDataModel(exchangeName);

                            string securityName = dataReader.GetField<string>("SecurityName");
                            string symbol = dataReader.GetField<string>("Symbol");
                            var securityData = new SecurityStaticDataModel(exchangeData, symbol, securityName);

                            securityStaticDataCollection.Add(securityData);
                        }
                    }
                }
            }

            return securityStaticDataCollection;
        }

        public void ClearExistingEndOfDayData(string symbol)
        {
            _logger.Information($"PersistenceManager: clearing existing stock data for symbol: {symbol}");

            int? securityId = GetSecurityIdBySymbol(symbol);
            if (!securityId.HasValue)
            {
                throw new ArgumentException($"Security not found for symbol: {symbol}");
            }

            string commandString = "DELETE FROM EndOfDayData WHERE SecurityId = @SecurityId";
            var parameters = new Dictionary<string, object> { { "@SecurityId", securityId.Value } };
            ExecuteNoReadQuery(commandString, -1, parameters);
        }

        public void Dispose()
        {
            _connectionWrapper.Close();
            _connectionWrapper.Dispose();
        }

        #endregion

        #region Private Methods

        private void ExecuteNoReadQuery(string commandString, int expectedAffectedRows, Dictionary<string, object> parameters)
        {
            lock (_lockObject)
            {
                using (IMySqlCommandWrapper command = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
                {
                    parameters.ToList().ForEach(x => command.AddParameter(x.Key, x.Value));

                    int affactedRows = command.ExecuteNonQuery();
                    if (expectedAffectedRows > -1 && affactedRows != expectedAffectedRows)
                    {
                        throw new InvalidOperationException($"Query execution failure, expected row count of 1, received {affactedRows}");
                    }
                }
            }
        }

        private int? GetExchangeIdByName(string name)
        {
            string commandString = "SELECT Id FROM ExchangeDetails WHERE Name = @ExchangeName;";

            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
            {
                commandWrapper.AddParameter("@ExchangeName", name);

                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        if (!dataReader.HasRows || !dataReader.ReadNext())
                        {
                            return null;
                        }

                        return dataReader.GetField<int?>("Id");
                    }
                }
            }
        }

        private int? GetSecurityIdBySymbol(string symbol)
        {
            string commandString = "SELECT Id FROM SecurityDetails WHERE Symbol = @Symbol;";

            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(commandString, _connectionWrapper))
            {
                commandWrapper.AddParameter("@Symbol", symbol);

                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        if (!dataReader.HasRows || !dataReader.ReadNext())
                        {
                            return null;
                        }

                        return dataReader.GetField<int?>("Id");
                    }
                }
            }
        }

        #endregion
    }
}
