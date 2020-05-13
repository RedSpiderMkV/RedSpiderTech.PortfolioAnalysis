using System.Xml.Serialization;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Model
{
    public class HoldingValuationDataReportingModel
    {
        #region Properties

        [XmlElement(ElementName = "Symbol")]
        public string StockSymbol { get; set; }
        
        [XmlElement(ElementName = "Valuation")]
        public decimal HoldingValue { get; set; }

        #endregion
    }
}
