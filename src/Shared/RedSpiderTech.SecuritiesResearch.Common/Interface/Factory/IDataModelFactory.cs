using RedSpiderTech.Securities.DataRetriever.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Common.Interface.Factory
{
    public interface IDataModelFactory
    {
        IExchangeStaticDataModel GetExchangeStaticDataModel(ISecurityStaticData securityStaticData);
        ISecurityStaticDataModel GetSecurityStaticDataModel(ISecurityStaticData securityStaticData);
        IStockDataModel GetStockDataModel(ISecurityEndOfDayData securityEndOfDayData);
    }
}