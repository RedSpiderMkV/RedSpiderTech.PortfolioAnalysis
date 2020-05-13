using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;
using Serilog;

namespace RedSpiderTech.Simulation.Statistics.Calculation
{
    public class StatisticsCalculationManager : IStatisticsCalculationManager
    {
        #region Private Data

        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public StatisticsCalculationManager(ILogger logger)
        {
            _logger = logger;

            _logger.Information("StatisticsCalculationManager: Instantiation successful.");
        }

        public IPortfolioStatisticsData GetPortfolioStatisticsData(IPortfolioValuationHistory portfolioValuationHistory)
        {
            IEnumerable<double> portfolioValuationData = portfolioValuationHistory.ValuationData.Select(x => (x.Valuation));
            IEnumerable<double> portfolioDailyReturnsData = GetPortfolioValuationDailyReturnsData(portfolioValuationData.ToList());

            double standardDeviation = portfolioDailyReturnsData.StandardDeviation();
            double portfolioReturn = GetTotalReturnAsPercentage_NoDividend(portfolioValuationData);

            double initialValuation = portfolioValuationHistory.InitialValuation;
            double finalValuation = portfolioValuationData.Last();
            double returnsToVolatilityRatio = portfolioReturn / standardDeviation;
            var portfolioStatistics = new PortfolioStatisticsData(portfolioValuationHistory.PortfolioName,
                                                                  portfolioValuationHistory.StartDate,
                                                                  portfolioValuationHistory.EndDate,
                                                                  initialValuation,
                                                                  finalValuation,
                                                                  portfolioReturn,
                                                                  standardDeviation,
                                                                  returnsToVolatilityRatio);

            return portfolioStatistics;
        }

        #endregion

        #region Private Methods

        private static IEnumerable<double> GetPortfolioValuationDailyReturnsData(List<double> dailyValuations)
        {
            var dailyReturns = new List<double>();
            dailyReturns.Add(0);

            for (int i = 1; i < dailyValuations.Count; i++)
            {
                double returnPercentage = (dailyValuations[i] - dailyValuations[i - 1]) / dailyValuations[i - 1] * 100;
                dailyReturns.Add(returnPercentage);
            }

            return dailyReturns;
        }

        private static double GetTotalReturnAsPercentage_NoDividend(IEnumerable<double> portfolioReturns)
        {
            double initialValue = portfolioReturns.First();
            double endValue = portfolioReturns.Last();

            double returns = (endValue - initialValue) / initialValue;
            return returns * 100;
        }

        #endregion
    }
}
