using System;
using System.Xml.Serialization;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Model
{
    public class PortfolioValuationDataReportingModel
    {
        #region Properties

        [XmlElement(ElementName = "ValuationDate")]
        public DateTime ValuationDate { get; set; }

        [XmlElement(ElementName = "PortfolioValuation")]
        public decimal PortfolioValuation { get; set; }

        [XmlArray("Holdings")]
        [XmlArrayItem(ElementName = "Holding")]
        public HoldingValuationDataReportingModel[] HoldingValuations { get; set; }

        #endregion
    }
}
