using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using RedSpiderTech.Securities.DataRetriever.Core;
using RedSpiderTech.Securities.DataRetriever.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Factory;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Enums;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution
{
    public class FileBasedDataGrabTask : IDataGrabTask
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

        public FileBasedDataGrabTask(ILogger logger, IContainer container, IDataArgs dataArgs)
        {
            _logger = logger;
            _container = container;
            _dataArgs = dataArgs;

            _securityDataRetriever = _container.Resolve<ISecurityDataRetriever>();
            _persistenceManager = _container.Resolve<IPersistenceManager>();
            _dataModelFactory = _container.Resolve<IDataModelFactory>();

            _logger.Information("FileBaseDataGrabTask: Instantiation successful.");
        }

        public void Execute()
        {
            _logger.Information("FileBasedDataGrabTask: Starting file based data grab execution.");

            try
            {
                string filePath = ((FileBaseDataArgs)_dataArgs).FilePath;
                string[] symbolCollection = File.ReadAllLines(filePath);

                IEnumerable<string> cleanedSymbolCollection = symbolCollection.Select(x => x.TrimEnd());
                IEnumerable<ISecurityStaticData> staticDataCollection = _securityDataRetriever.GetSecurityStaticData(cleanedSymbolCollection);

                ProcessExchangeStaticData(staticDataCollection);
                ProcessSecurityStaticData(staticDataCollection);
                ProcessEndOfDayData(cleanedSymbolCollection);
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

        private void ProcessEndOfDayData(IEnumerable<string> symbols)
        {
            foreach(string symbol in symbols)
            {
                IEnumerable<ISecurityEndOfDayData> historicData = _securityDataRetriever.GetAllSecurityHistoricData(symbol);
                IEnumerable<IStockDataModel> historicDataModels = historicData.Select(_dataModelFactory.GetStockDataModel);
                InsertSecurityEndOfDayData(historicDataModels);
            }
        }

        private void ProcessSecurityStaticData(IEnumerable<ISecurityStaticData> staticDataCollection)
        {
            IEnumerable<ISecurityStaticDataModel> securityStaticDataModels = staticDataCollection.Select(_dataModelFactory.GetSecurityStaticDataModel);
            securityStaticDataModels.ToList().ForEach(InsertSecurityStaticeDataIfNotExists);
        }

        private void ProcessExchangeStaticData(IEnumerable<ISecurityStaticData> staticDataCollection)
        {
            IEnumerable<IExchangeStaticDataModel> exchangeStaticDataModels = staticDataCollection.Select(_dataModelFactory.GetExchangeStaticDataModel);
            IEnumerable<IExchangeStaticDataModel> uniqueExchangeModels = exchangeStaticDataModels.GroupBy(x => x.ExchangeName, (key, group) => group.First());
            uniqueExchangeModels.ToList().ForEach(InsertExchangeDataIfNotExists);
        }

        private void InsertSecurityEndOfDayData(IEnumerable<IStockDataModel> stockDataModelCollection)
        {
            try
            {
                string symbol = stockDataModelCollection.First().Symbol;

                _logger.Information($"FileBasedDataGrabTask: Clearing existing end of day data for: {symbol}");
                _persistenceManager.ClearExistingEndOfDayData(symbol);

                _logger.Information($"FileBasedDataGrabTask: Inserting end of day data for: {symbol}");
                _persistenceManager.InsertEndOfDayData(stockDataModelCollection, VendorDetails.YahooFinance);
            }
            catch(Exception exception)
            {
                _logger.Error($"FileBasedDataGrabTask: Error retrieving stock data.");
                _logger.Error(exception.ToString());
            }
        }

        private void InsertExchangeDataIfNotExists(IExchangeStaticDataModel exchangeStaticDataModel)
        {
            try
            {
                _logger.Information($"FileBasedDataGrabTask: Inserting exchange data if not exists for exchange: {exchangeStaticDataModel.ExchangeName}");
                if (!_persistenceManager.DoesExchangeExist(exchangeStaticDataModel))
                {
                    _persistenceManager.InsertExchange(exchangeStaticDataModel);
                }
            }
            catch(Exception exception)
            {
                _logger.Error($"FileBasedDataGrabTask: Error processing exchange data for exchange: {exchangeStaticDataModel.ExchangeName}");
                _logger.Error(exception.ToString());
            }
        }

        private void InsertSecurityStaticeDataIfNotExists(ISecurityStaticDataModel securityStaticDataModel)
        {
            try
            {
                _logger.Information($"FileBasedDataGrabTask: Inserting static data if not exists for symbol: {securityStaticDataModel.Symbol}");
                if (!_persistenceManager.DoesSecurityStaticDataExist(securityStaticDataModel))
                {
                    _persistenceManager.InsertSecurityStatic(securityStaticDataModel);
                }
            }
            catch(Exception exception)
            {
                _logger.Error($"FileBasedDataGrabTask: Error processing static data for symbol: {securityStaticDataModel.Symbol}");
                _logger.Error(exception.ToString());
            }
        }

        #endregion
    }
}
