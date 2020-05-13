using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedSpiderTech.SecuritiesResearch.Core.Interface;
using Serilog;
using YahooFinanceApi;

namespace RedSpiderTech.SecuritiesResearch.Core.Implementation
{
    public class YahooFinanceApiManager : IYahooFinanceApiManager
    {
        #region Private Data

        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public YahooFinanceApiManager(ILogger logger)
        {
            _logger = logger;
        }

        public IReadOnlyList<Candle> GetSecurityHistoricData(string symbol, DateTime startDate, DateTime endDate)
        {
            _logger.Information($"YahooFinanceApiManager: Retrieving historic data for {symbol} between {startDate.ToString("yyyy-MM-dd")}" +
                $" and {endDate.ToString("yyyy-MM-dd")}");

            Task<IReadOnlyList<Candle>> dataTask = Yahoo.GetHistoricalAsync(symbol, startDate, endDate, Period.Daily);
            dataTask.Wait();

            return dataTask?.Result;
        }

        public Security GetSecurityStaticData(string symbol)
        {
            _logger.Information($"YahooFinanceApiManager: Retrieving static data for {symbol}");

            Task<IReadOnlyDictionary<string, Security>> marketDataTask = Yahoo
                .Symbols(symbol)
                .Fields(Field.Symbol, Field.ShortName, Field.Exchange, Field.FullExchangeName)
                .QueryAsync();
            marketDataTask.Wait();

            return marketDataTask.Result[symbol];
        }

        #endregion
    }
}
