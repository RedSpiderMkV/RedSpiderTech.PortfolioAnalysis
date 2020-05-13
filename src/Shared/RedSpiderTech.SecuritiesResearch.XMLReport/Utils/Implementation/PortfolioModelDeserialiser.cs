using System.IO;
using System.Xml.Serialization;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Interface;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Implementation
{
    public class PortfolioModelDeserialiser : IPortfolioModelDeserialiser
    {
        #region Private Data

        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public PortfolioModelDeserialiser(ILogger logger)
        {
            _logger = logger;
        }

        public PortfolioValuationSummaryDataModel GetReportDataModel(string xmlReportFile)
        {
            _logger.Information($"PortfolioModelDeserialiser: Building data from file: {xmlReportFile}");

            using (FileStream fileStream = new FileStream(xmlReportFile, FileMode.Open))
            {
                var writer = new XmlSerializer(typeof(PortfolioValuationSummaryDataModel));
                return writer.Deserialize(fileStream) as PortfolioValuationSummaryDataModel;
            }
        }

        #endregion
    }
}
