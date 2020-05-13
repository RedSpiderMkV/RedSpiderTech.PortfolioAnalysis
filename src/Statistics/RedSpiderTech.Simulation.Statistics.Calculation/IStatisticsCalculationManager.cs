using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Statistics.Calculation
{
    public interface IStatisticsCalculationManager
    {
        IPortfolioStatisticsData GetPortfolioStatisticsData(IPortfolioValuationHistory portfolioValuationHistory);
    }
}