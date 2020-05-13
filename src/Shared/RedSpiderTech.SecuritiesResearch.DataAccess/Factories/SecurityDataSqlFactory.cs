using System;
using RedSpiderTech.SecuritiesResearch.Common.Implementation.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Factories
{
    public class SecurityDataSqlFactory : ISecurityDataSqlFactory
    {
        public IStockDataModel GetStockData(IMySqlDataReaderWrapper dataReader)
        {
            string symbol = dataReader.GetField<string>("symbol");
            DateTime dateStamp = dataReader.GetField<DateTime>("DateStamp");
            decimal open = (decimal)dataReader.GetField<Single>("Open");
            decimal high = (decimal)dataReader.GetField<Single>("High");
            decimal low = (decimal)dataReader.GetField<Single>("Low");
            decimal close = (decimal)dataReader.GetField<Single>("Close");
            decimal adjustedClose = (decimal)dataReader.GetField<Single>("AdjustedClose");
            long volume = dataReader.GetField<long>("Volume");
            decimal dayChange = (decimal)dataReader.GetField<Single>("DayChange");
            decimal dayPercentageChange = (decimal)dataReader.GetField<Single>("DayPercentageChange");
            decimal standardChange = (decimal)dataReader.GetField<Single>("StandardChange");
            decimal standardPercentageChange = (decimal)dataReader.GetField<Single>("StandardPercentageChange");

            var stockData = new StockDataModel(symbol, dateStamp, open, high, low, close, volume, adjustedClose, dayChange, dayPercentageChange, standardChange, standardPercentageChange);
            return stockData;
        }
    }
}
