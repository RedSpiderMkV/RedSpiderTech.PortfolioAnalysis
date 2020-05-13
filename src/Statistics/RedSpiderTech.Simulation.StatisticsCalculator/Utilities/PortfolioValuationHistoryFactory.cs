using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Interface;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;
using Serilog;

namespace RedSpiderTech.Simulation.Statistics.Host.Utilities
{
    public class PortfolioValuationHistoryFactory : IPortfolioValuationHistoryFactory
    {
        #region Private Data

        private readonly ILogger _logger;
        private readonly IPortfolioModelDeserialiser _portfolioModelDeserialiser;

        #endregion

        #region Public Methods

        public PortfolioValuationHistoryFactory(ILogger logger, IPortfolioModelDeserialiser portfolioModelDeserialiser)
        {
            _logger = logger;
            _portfolioModelDeserialiser = portfolioModelDeserialiser;

            _logger.Information("PortfolioValuationHistoryFactory: Instantiation successful.");
        }

        public IPortfolioValuationHistory GetPortfolioValuationHistory(string filePath)
        {
            PortfolioValuationSummaryDataModel portfolioModel = _portfolioModelDeserialiser.GetReportDataModel(filePath);

            var portfolioValuationHistory = new PortfolioValuationHistory(portfolioModel.MetaData.PortfolioName, (double)portfolioModel.MetaData.InitialBalance);
            IEnumerable<IValuationData> valuationData = portfolioModel.PortfolioValuationData.Select(x => new ValuationData(x.ValuationDate, (double)x.PortfolioValuation));
            valuationData.ToList().ForEach(portfolioValuationHistory.AddValuationData);

            return portfolioValuationHistory;
        }

        #endregion
    }
}
