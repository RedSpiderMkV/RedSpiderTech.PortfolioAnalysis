using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Reporting.Interface;
using RedSpiderTech.Simulation.Reporting.Interface.Factory;
using RedSpiderTech.Simulation.Reporting.Interface.Utilities;
using RedSpiderTech.Simulation.Simulations.Common;
using RedSpiderTech.Simulation.Statistics.Calculation;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;
using Serilog;

namespace RedSpiderTech.Simulation.Reporting.Implementation
{
    public class PortfolioValuationReporter : IPortfolioValuationReporter
    {
        #region Private Data

        private readonly ILogger _logger;
        private readonly INotificationManager _notificationManager;
        private readonly ISimulationTypeManager _simulationTypeManager;
        private readonly IXmlModelFactory _xmlModelFactory;
        private readonly IPortfolioValuationSummaryReportWriter _xmlWriter;
        private readonly IStatisticsCalculationManager _statisticsCalculationManager;
        private readonly PortfolioValuationSummaryDataModel _portfolioValuationSummary;

        #endregion

        #region Public Methods

        public PortfolioValuationReporter(ILogger logger, 
                                          INotificationManager notificationManager, 
                                          IStatisticsCalculationManager statisticsCalculationManager,
                                          ISimulationTypeManager simulationTypeManager, 
                                          IXmlModelFactory xmlModelFactory, 
                                          IPortfolioValuationSummaryReportWriter xmlWriter)
        {
            _logger = logger;
            _statisticsCalculationManager = statisticsCalculationManager;
            _notificationManager = notificationManager;
            _simulationTypeManager = simulationTypeManager;
            _xmlModelFactory = xmlModelFactory;
            _portfolioValuationSummary = _xmlModelFactory.GetValuationSummaryDataModel();
            _xmlWriter = xmlWriter;

            _logger.Information("HoldingsReporter: Instantiation successful.");
        }

        public void Dispose()
        {
            _notificationManager.SimulationStarted -= NotificationManager_SimulationStarted;
            _notificationManager.SimulationEnded -= NotificationManager_SimulationEnded;
            _simulationTypeManager.SimulationManager.AccountUpdatedEvent -= RebalanceSimulationManager_AccountUpdatedEvent;
        }

        public void Start()
        {
            _notificationManager.SimulationStarted += NotificationManager_SimulationStarted;
            _notificationManager.SimulationEnded += NotificationManager_SimulationEnded;
        }

        #endregion

        #region Private Methods

        private void NotificationManager_SimulationStarted(object sender, EventArgs e)
        {
            _simulationTypeManager.SimulationManager.AccountUpdatedEvent += RebalanceSimulationManager_AccountUpdatedEvent;
        }

        private void RebalanceSimulationManager_AccountUpdatedEvent(object sender, AccountUpdatedEventArgs eventArgs)
        {
            PortfolioValuationDataReportingModel portfolioValuation = _xmlModelFactory.GetValuationDataReportingModel(eventArgs.ValuationDate, eventArgs.Account);
            _portfolioValuationSummary.AppendValuationModel(portfolioValuation);
        }

        private void NotificationManager_SimulationEnded(object sender, EventArgs e)
        {
            _logger.Information("Statistics");
            EnrichReportingModelWithStatistics();

            _xmlWriter.Write(_portfolioValuationSummary);
        }

        private void EnrichReportingModelWithStatistics()
        {
            var portfolioValuationHistory = new PortfolioValuationHistory(_portfolioValuationSummary.MetaData.PortfolioName, (double)_portfolioValuationSummary.MetaData.InitialBalance);
            IEnumerable<IValuationData> valuationData = _portfolioValuationSummary.PortfolioValuationData.Select(x => new ValuationData(x.ValuationDate, (double)x.PortfolioValuation));
            valuationData.ToList().ForEach(portfolioValuationHistory.AddValuationData);

            IPortfolioStatisticsData statisticsCalculationData = _statisticsCalculationManager.GetPortfolioStatisticsData(portfolioValuationHistory);

            _logger.Information($"Portfolio run from {statisticsCalculationData.StartDate.ToString("yyyy-MM-dd")} to {statisticsCalculationData.EndDate.ToString("yyyy-MM-dd")}");
            _logger.Information($"Portfolio name: {statisticsCalculationData.PortfolioIdentifier}");
            _logger.Information(string.Format("Initial valuation: {0:0.00}", statisticsCalculationData.InitialValuation));
            _logger.Information(string.Format("Final valuation: {0:0.00}", statisticsCalculationData.FinalValuation));
            _logger.Information(string.Format("Portfolio Total Returns: {0:0.00} %", statisticsCalculationData.TotalReturns));
            _logger.Information(string.Format("Portfolio Standard Deviation: {0:0.00} %", statisticsCalculationData.DailyReturnsVolatility));
            _logger.Information(string.Format("Returns to Volatility ratio: {0:0.00}", statisticsCalculationData.ReturnsToVolatilityRatio));

            PortfolioRunStatistics portfolioStatistics = _xmlModelFactory.GetPortfolioRunStatistics(statisticsCalculationData);
            _portfolioValuationSummary.RunStatistics = portfolioStatistics;
        }

        #endregion
    }
}
