using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Core
{
    public interface IYahooFinanceDataRepository
    {
        ISecurityStaticData GetSecurityStaticData(string symbol);
        IEnumerable<IStockData> GetHistoricData(string symbol, DateTime startDate, DateTime endDate);
    }
}