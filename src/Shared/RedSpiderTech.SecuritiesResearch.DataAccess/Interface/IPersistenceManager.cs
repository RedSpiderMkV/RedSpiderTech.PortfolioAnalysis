using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Enums;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface IPersistenceManager : IDisposable
    {
        bool DoesExchangeExist(IExchangeStaticDataModel exchangeStaticData);
        bool DoesSecurityStaticDataExist(ISecurityStaticDataModel securityStaticData);
        void InsertExchange(IExchangeStaticDataModel exchangeStaticData);
        void InsertSecurityStatic(ISecurityStaticDataModel securityStaticData);
        void InsertEndOfDayData(IEnumerable<IStockDataModel> stockDataCollection, VendorDetails vendorDetails);
        void ClearExistingEndOfDayData(string symbol);
        DateTime GetLatestStockDateStamp(string symbol);
        IEnumerable<ISecurityStaticDataModel> GetAllExistingStaticData();
    }
}