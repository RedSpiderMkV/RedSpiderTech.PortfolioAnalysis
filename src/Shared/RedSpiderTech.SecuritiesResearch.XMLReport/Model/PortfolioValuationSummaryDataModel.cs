using System.Collections.Generic;
using System.Xml.Serialization;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Model
{
    [XmlRoot(ElementName = "PortfolioValuationSummary")]
    public class PortfolioValuationSummaryDataModel
    {
        #region Properties

        [XmlElement(ElementName = "PortfolioMetaData")]
        public PortfolioMetaData MetaData { get; set; }

        [XmlElement(ElementName = "PortfolioRunStatistics")]
        public PortfolioRunStatistics RunStatistics { get; set; }

        [XmlArray("PortfolioValuationHistory")]
        [XmlArrayItem(ElementName = "PortfolioValuation")]
        public List<PortfolioValuationDataReportingModel> PortfolioValuationData { get; }

        #endregion

        #region Public Methods

        public PortfolioValuationSummaryDataModel()
        {
            PortfolioValuationData = new List<PortfolioValuationDataReportingModel>();
        }

        public void AppendValuationModel(PortfolioValuationDataReportingModel dataModel)
        {
            PortfolioValuationData.Add(dataModel);
        }

        #endregion
    }
}
