using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Factory;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.Core.Interface;
using Serilog;
using YahooFinanceApi;

namespace RedSpiderTech.SecuritiesResearch.Core
{
    public class YahooFinanceDataRepository : IYahooFinanceDataRepository
    {
        #region Private Data

        private readonly IYahooFinanceApiManager _yahooFinanceApiManager;
        private readonly ISecurityDataFactory _securityDataFactory;
        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public YahooFinanceDataRepository(
            IYahooFinanceApiManager yahooFinanceApiManager, 
            ISecurityDataFactory securityDataFactory,
            ILogger logger)
        {
            _yahooFinanceApiManager = yahooFinanceApiManager;
            _securityDataFactory = securityDataFactory;
            _logger = logger;
        }

        public ISecurityStaticData GetSecurityStaticData(string symbol)
        {
            Security securityData = _yahooFinanceApiManager.GetSecurityStaticData(symbol);

            return _securityDataFactory.GetStaticData(securityData);
        }

        public IEnumerable<IStockData> GetHistoricData(string symbol, DateTime startDate, DateTime endDate)
        {
            IReadOnlyList<Candle> historicData = _yahooFinanceApiManager.GetSecurityHistoricData(symbol, startDate, endDate);
            if(historicData == null)
            {
                _logger.Error($"No data retrieved from Api manager for symbol: {symbol}");
                return Enumerable.Empty<IStockData>();
            }

            List<Candle> orderedHistoricData = historicData.OrderBy(x => x.DateTime).ToList();
            var historicStockData = new List<IStockData>();

            IStockData initialData = _securityDataFactory.GetStockData(symbol, orderedHistoricData[0], null);
            historicStockData.Add(initialData);
            for(int i = 1; i < orderedHistoricData.Count; i++)
            {
                //if(!IsValid(orderedHistoricData[i]))
                //{
                //    _logger.Error($"Data point not valid - index: {i} Date: {orderedHistoricData[i].DateTime.ToString("yyyy-MM-dd")}");
                //    continue;
                //}

                decimal? previousClose = orderedHistoricData[i - 1].Close == 0 ? null : (decimal?)orderedHistoricData[i - 1].Close;
                IStockData stockData = _securityDataFactory.GetStockData(symbol, orderedHistoricData[i], previousClose);
                historicStockData.Add(stockData);
            }

            return historicStockData;
        }

        #endregion

        #region Private Methods

        private static bool IsValid(Candle candle)
        {
            if(candle.Open == 0 || candle.Close == 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
