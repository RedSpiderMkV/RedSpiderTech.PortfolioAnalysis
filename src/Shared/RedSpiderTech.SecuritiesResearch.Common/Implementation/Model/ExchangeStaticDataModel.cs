using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Common.Implementation.Model
{
    public class ExchangeStaticDataModel : IExchangeStaticDataModel
    {
        #region Properties

        public string ExchangeName { get; }

        #endregion

        #region Public Methods

        public ExchangeStaticDataModel(string name)
        {
            ExchangeName = name;
        }

        public override string ToString()
        {
            return $"ExchangeName: {ExchangeName}";
        }

        #endregion
    }
}
