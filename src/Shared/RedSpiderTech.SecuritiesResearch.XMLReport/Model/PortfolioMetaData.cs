using System.Xml.Serialization;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Model
{
    public class PortfolioMetaData
    {
        #region Properties

        [XmlElement(ElementName = "PortfolioName")]
        public string PortfolioName { get; set; }

        [XmlElement(ElementName = "RebalanceEnabled")]
        public bool RebalanceEnabled { get; set; }

        [XmlElement(ElementName = "InitialBalance")]
        public decimal InitialBalance { get; set; }

        [XmlElement(ElementName = "PortfolioCurrency")]
        public string Currency { get; set; }

        #endregion
    }
}
