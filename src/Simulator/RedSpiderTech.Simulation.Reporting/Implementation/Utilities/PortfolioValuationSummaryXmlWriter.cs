using System.IO;
using System.Xml.Serialization;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Reporting.Interface.Utilities;
using Serilog;

namespace RedSpiderTech.Simulation.Reporting.Implementation.Utilities
{
    public class PortfolioValuationSummaryXmlWriter : IPortfolioValuationSummaryReportWriter
    {
        #region Private Data

        private readonly IHashingFilenameGenerator _hashingFilenameGenerator;
        private readonly ILogger _logger;
        private readonly string _outputDirectory;

        #endregion

        #region Public Methods

        public PortfolioValuationSummaryXmlWriter(ILogger logger, IAppConfigurationManager appConfigurationManager, IHashingFilenameGenerator hashingFilenameGenerator)
        {
            _hashingFilenameGenerator = hashingFilenameGenerator;
            _outputDirectory = appConfigurationManager.SimulationReportDirectory;
            _logger = logger;
        }

        public void Write(PortfolioValuationSummaryDataModel portfolioValuationSummary)
        {
            _logger.Information("PortfolioValuationSummaryXmlWriter: Writing to summary file.");

            string fileNameWithPath = Path.Combine(_outputDirectory, _hashingFilenameGenerator.GetFilename());
            _logger.Information($"PortfolioValuationSummaryXmlWriter: Writing to filepath: {fileNameWithPath}");

            using (FileStream file = File.Create(fileNameWithPath))
            {
                var writer = new XmlSerializer(portfolioValuationSummary.GetType());
                writer.Serialize(file, portfolioValuationSummary);
            }
        }

        #endregion
    }
}
