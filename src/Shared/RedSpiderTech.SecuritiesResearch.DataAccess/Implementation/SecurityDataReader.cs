using System;
using System.Collections.Generic;
using System.Text;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Implementation
{
    public class SecurityDataReader : ISecurityDataReader
    {
        #region Private Data

        private readonly IMySqlConnectionWrapper _connectionWrapper;
        private readonly IMySqlCommandWrapperFactory _commandWrapperFactory;
        private readonly ISecurityDataSqlFactory _securityDataSqlFactory;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Public Methods

        public SecurityDataReader(
            IMySqlConnectionWrapper connectionWrapper,
            IMySqlCommandWrapperFactory commandWrapperFactory,
            ISecurityDataSqlFactory securityDataSqlFactory,
            ILogger logger)
        {
            _logger = logger;
            _connectionWrapper = connectionWrapper;
            _commandWrapperFactory = commandWrapperFactory;
            _securityDataSqlFactory = securityDataSqlFactory;

            _logger.Information("SecurityDataReader: Opening database connection.");
            _connectionWrapper.Open();
            _logger.Information("SecurityDataReader: Connection open.");
        }

        public IEnumerable<IStockDataModel> GetSecurityData(IEnumerable<string> symbolCollection, DateTime startDateStamp, DateTime endDateStamp)
        {
            StringBuilder stringBuilder = new StringBuilder("WHERE sd.symbol IN (");
            foreach(string symbol in symbolCollection)
            {
                stringBuilder.Append($"'{symbol}',");
            }

            stringBuilder.Length--;
            stringBuilder.Append(")");
            string symolFilter = stringBuilder.ToString();

            string command = @"select sd.symbol, 
                               eod.Open,
                               eod.High,
                               eod.Low,
                               eod.Close,
                               eod.AdjustedClose,
                               eod.Volume,
                               eod.DayChange,
                               eod.DayPercentageChange,
                               eod.StandardChange,
                               eod.StandardPercentageChange,
                               eod.DateStamp from endofdaydata eod
                               join securitydetails sd on sd.Id = eod.SecurityId
                               "
                               + symolFilter + Environment.NewLine +
                               $"and eod.DateStamp >= '{startDateStamp.ToString("yyyy-MM-dd")}' and eod.DateStamp <= '{endDateStamp.ToString("yyyy-MM-dd")}'" + Environment.NewLine +
                               "order by eod.DateStamp ASC;";

            var stockDataCollection = new List<IStockDataModel>();
            using (IMySqlCommandWrapper commandWrapper = _commandWrapperFactory.GetCommandWrapper(command, _connectionWrapper))
            {
                lock (_lockObject)
                {
                    using (IMySqlDataReaderWrapper dataReader = commandWrapper.ExecuteReader())
                    {
                        if(!dataReader.HasRows)
                        {
                            return stockDataCollection;
                        }

                        while(dataReader.ReadNext())
                        {
                            IStockDataModel stockData = _securityDataSqlFactory.GetStockData(dataReader);
                            stockDataCollection.Add(stockData);
                        }
                    }
                }
            }

            return stockDataCollection;
        }

        public void Dispose()
        {
            _connectionWrapper.Close();
            _connectionWrapper.Dispose();
        }

        #endregion
    }
}
