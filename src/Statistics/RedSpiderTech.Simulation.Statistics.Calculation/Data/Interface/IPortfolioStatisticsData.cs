using System;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface
{
    public interface IPortfolioStatisticsData
    {
        string PortfolioIdentifier { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }

        double DailyReturnsVolatility { get; }
        double FinalValuation { get; }
        double InitialValuation { get; }
        double ReturnsToVolatilityRatio { get; }
        double TotalReturns { get; }
    }
}