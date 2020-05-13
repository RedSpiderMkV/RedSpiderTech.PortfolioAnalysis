using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Statistics.Host.Utilities
{
    public interface IPortfolioValuationHistoryFactory
    {
        IPortfolioValuationHistory GetPortfolioValuationHistory(string filePath);
    }
}