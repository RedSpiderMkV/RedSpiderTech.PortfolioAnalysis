
namespace RedSpiderTech.Simulation.Statistics.Host.Utilities
{
    public class StatisticsArguments : IStatisticsArguments
    {
        #region Properties

        public string PortfolioReturnsFile { get; }
        public string BenchmarkReturnsFile { get; }

        #endregion

        #region Public Methods

        public StatisticsArguments(string portfolioReturnsFile, string benchmarkReturnsFile)
        {
            PortfolioReturnsFile = portfolioReturnsFile;
            BenchmarkReturnsFile = benchmarkReturnsFile;
        }

        #endregion
    }
}
