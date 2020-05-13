using RedSpiderTech.Securities.DataRetriever.Model;
using RedSpiderTech.SecuritiesResearch.Common.Implementation.Model;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Factory;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Common.Implementation.Factory
{
    public class DataModelFactory : IDataModelFactory
    {
        public IExchangeStaticDataModel GetExchangeStaticDataModel(ISecurityStaticData securityStaticData)
        {
            var exchangeStaticDataModel = new ExchangeStaticDataModel(securityStaticData.ExchangeShortName);
            return exchangeStaticDataModel;
        }

        public ISecurityStaticDataModel GetSecurityStaticDataModel(ISecurityStaticData securityStaticData)
        {
            IExchangeStaticDataModel exchangeStaticDataModel = GetExchangeStaticDataModel(securityStaticData);
            var securityStaticDataModel = new SecurityStaticDataModel(exchangeStaticDataModel, securityStaticData.Symbol, securityStaticData.ShortName);

            return securityStaticDataModel;
        }

        public IStockDataModel GetStockDataModel(ISecurityEndOfDayData securityEndOfDayData)
        {
            var stockDataModel = new StockDataModel(securityEndOfDayData.Symbol,
                                                    securityEndOfDayData.TimeStamp,
                                                    securityEndOfDayData.Open,
                                                    securityEndOfDayData.High,
                                                    securityEndOfDayData.Low,
                                                    securityEndOfDayData.Close,
                                                    securityEndOfDayData.Volume,
                                                    securityEndOfDayData.AdjustedClose,
                                                    securityEndOfDayData.DayChange,
                                                    securityEndOfDayData.DayPercentageChange,
                                                    securityEndOfDayData.StandardChange ?? 0,
                                                    securityEndOfDayData.StandardPercentageChange ?? 0);
            
            return stockDataModel;
        }
    }
}
