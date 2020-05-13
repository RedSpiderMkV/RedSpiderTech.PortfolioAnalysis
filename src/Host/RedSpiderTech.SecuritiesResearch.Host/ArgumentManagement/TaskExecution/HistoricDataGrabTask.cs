using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using RedSpiderTech.Securities.DataRetriever.Core;
using RedSpiderTech.Securities.DataRetriever.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Factory;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Enums;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Enums;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution
{
    public class HistoricDataGrabTask : IDataGrabTask
    {
        #region Private Data

        private readonly IContainer _container;
        private readonly ILogger _logger;
        private readonly IDataArgs _dataArgs;
        private readonly ISecurityDataRetriever _securityDataRetriever;
        private readonly IPersistenceManager _persistenceManager;
        private readonly IDataModelFactory _dataModelFactory;

        #endregion

        #region Public Methods

        public HistoricDataGrabTask(ILogger logger, IContainer container, IDataArgs dataArgs)
        {
            _logger = logger;
            _container = container;
            _dataArgs = dataArgs;

            _securityDataRetriever = _container.Resolve<ISecurityDataRetriever>();
            _persistenceManager = _container.Resolve<IPersistenceManager>();
            _dataModelFactory = _container.Resolve<IDataModelFactory>();

            _logger.Information("HistoricDataGrabTask: Instantiation successful.");
        }

        public void Execute()
        {
            _logger.Information("HistoricDataGrabTask: Starting historic data grab execution.");

            try
            {
                string symbol = ((HistoricDataArgs)_dataArgs).Parameters[DataGrabParameters.Symbol];
                _logger.Information($"Symbol: {symbol}");

                string[] symbolCollection = new string[] { symbol };

                IEnumerable<string> cleanedSymbolCollection = symbolCollection.Select(x => x.TrimEnd());
                IEnumerable<ISecurityStaticData> staticDataCollection = _securityDataRetriever.GetSecurityStaticData(cleanedSymbolCollection);
                IEnumerable<IEnumerable<ISecurityEndOfDayData>> securityEndOfDayDataCollection = cleanedSymbolCollection.Select(_securityDataRetriever.GetAllSecurityHistoricData);
                IEnumerable<ISecurityEndOfDayData> allEndOfDayData = securityEndOfDayDataCollection.SelectMany(x => x);

                IEnumerable<IExchangeStaticDataModel> exchangeStaticDataModels = staticDataCollection.Select(_dataModelFactory.GetExchangeStaticDataModel);
                IEnumerable<ISecurityStaticDataModel> securityStaticDataModels = staticDataCollection.Select(_dataModelFactory.GetSecurityStaticDataModel);
                IEnumerable<IStockDataModel> allStockDataModel = allEndOfDayData.Select(_dataModelFactory.GetStockDataModel);
                IEnumerable<IGrouping<string, IStockDataModel>> groupedStockDataModelCollection = allStockDataModel.GroupBy(x => x.Symbol);

                exchangeStaticDataModels.ToList().ForEach(InsertExchangeDataIfNotExists);
                securityStaticDataModels.ToList().ForEach(InsertSecurityStaticeDataIfNotExists);
                groupedStockDataModelCollection.ToList().ForEach(x => InsertSecurityEndOfDayData(x));
            }
            catch (Exception exception)
            {
                _logger.Error($"FileBasedDataGrab: Error in task execution...");
                _logger.Error(exception.ToString());
            }
        }

        public void Dispose()
        {
            _persistenceManager.Dispose();
        }

        #endregion

        #region Private Methods

        private void InsertSecurityEndOfDayData(IEnumerable<IStockDataModel> stockDataModelCollection)
        {
            string symbol = stockDataModelCollection.First().Symbol;

            _logger.Information($"HistoricDataGrabTask: Clearing existing end of day data for: {symbol}");
            _persistenceManager.ClearExistingEndOfDayData(symbol);

            _logger.Information($"HistoricDataGrabTask: Inserting end of day data for: {symbol}");
            _persistenceManager.InsertEndOfDayData(stockDataModelCollection, VendorDetails.YahooFinance);
        }

        private void InsertExchangeDataIfNotExists(IExchangeStaticDataModel exchangeStaticDataModel)
        {
            _logger.Error($"HistoricDataGrabTask: Inserting exchange data if not exists for exchange: {exchangeStaticDataModel.ExchangeName}");
            if (!_persistenceManager.DoesExchangeExist(exchangeStaticDataModel))
            {
                _persistenceManager.InsertExchange(exchangeStaticDataModel);
            }
        }

        private void InsertSecurityStaticeDataIfNotExists(ISecurityStaticDataModel securityStaticDataModel)
        {
            _logger.Error($"HistoricDataGrabTask: Inserting static data if not exists for symbol: {securityStaticDataModel.Symbol}");
            if (!_persistenceManager.DoesSecurityStaticDataExist(securityStaticDataModel))
            {
                _persistenceManager.InsertSecurityStatic(securityStaticDataModel);
            }
        }

        #endregion
    }
}
