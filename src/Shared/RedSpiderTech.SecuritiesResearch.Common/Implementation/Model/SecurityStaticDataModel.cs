using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Common.Implementation.Model
{
    public class SecurityStaticDataModel : ISecurityStaticDataModel
    {
        #region Properties

        public IExchangeStaticDataModel ExchangeData { get; }

        public string Symbol { get; }

        public string ShortName { get; }

        #endregion

        #region Public Methods

        public SecurityStaticDataModel(IExchangeStaticDataModel exchangeStaticData, string symbol, string name)
        {
            ShortName = name;
            Symbol = symbol;
            ExchangeData = exchangeStaticData;
        }

        public override string ToString()
        {
            return $"{ExchangeData.ToString()}\nSymbol: {Symbol}\nSecurityName: {ShortName}";
        }

        #endregion
    }
}
