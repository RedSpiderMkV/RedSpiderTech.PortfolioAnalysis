using System.Xml.Serialization;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Model
{
    public class PortfolioRunStatistics
    {
        #region Properties

        [XmlElement(ElementName = "InitialValuation")]
        public double InitialValuation { get; set; }

        [XmlElement(ElementName = "FinalValuation")]
        public double FinalValuation { get; set; }

        [XmlElement(ElementName = "TotalReturns")]
        public double TotalReturns { get; set; }

        [XmlElement(ElementName = "DailyReturnsVolatility")]
        public double DailyReturnsVolatility { get; set; }

        [XmlElement(ElementName = "ReturnsToVolatilityRatio")]
        public double ReturnsToVolatilityRatio { get; set; }

        #endregion
    }
}
