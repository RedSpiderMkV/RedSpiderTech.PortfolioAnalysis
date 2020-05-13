using System;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation
{
    public class PortfolioStatisticsData : IPortfolioStatisticsData
    {
        #region Properties

        public string PortfolioIdentifier { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public double InitialValuation { get; }
        public double FinalValuation { get; }
        public double TotalReturns { get; }
        public double DailyReturnsVolatility { get; }
        public double ReturnsToVolatilityRatio { get; }

        #endregion

        #region Public Methods

        public PortfolioStatisticsData(string portfolioName, 
                                       DateTime startDate, 
                                       DateTime endDate, 
                                       double initialValuation, 
                                       double finalValuation, 
                                       double totalReturns, 
                                       double dailyReturnsVolatility, 
                                       double returnsToVolatilityRatio)
        {
            PortfolioIdentifier = portfolioName;
            StartDate = startDate;
            EndDate = endDate;

            InitialValuation = initialValuation;
            FinalValuation = finalValuation;
            TotalReturns = totalReturns;
            DailyReturnsVolatility = dailyReturnsVolatility;
            ReturnsToVolatilityRatio = returnsToVolatilityRatio;
        }

        #endregion
    }
}
