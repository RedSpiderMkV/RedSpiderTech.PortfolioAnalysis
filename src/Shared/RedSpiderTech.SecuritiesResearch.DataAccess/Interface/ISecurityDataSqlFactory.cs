using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface ISecurityDataSqlFactory
    {
        IStockDataModel GetStockData(IMySqlDataReaderWrapper dataReader);
    }
}