using System;
using System.Collections.Generic;
using YahooFinanceApi;

namespace RedSpiderTech.SecuritiesResearch.Core.Interface
{
    public interface IYahooFinanceApiManager
    {
        IReadOnlyList<Candle> GetSecurityHistoricData(string symbol, DateTime startDate, DateTime endDate);
        Security GetSecurityStaticData(string symbol);
    }
}