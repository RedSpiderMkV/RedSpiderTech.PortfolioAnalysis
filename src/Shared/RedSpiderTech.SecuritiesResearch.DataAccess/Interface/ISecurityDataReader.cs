using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface ISecurityDataReader : IDisposable
    {
        IEnumerable<IStockDataModel> GetSecurityData(IEnumerable<string> symbolCollection, DateTime startDateStamp, DateTime endDateStamp);
    }
}