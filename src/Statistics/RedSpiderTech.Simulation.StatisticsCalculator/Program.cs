using Autofac;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Implementation;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Interface;
using RedSpiderTech.Simulation.Statistics.Calculation;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;
using RedSpiderTech.Simulation.Statistics.Common.Implementation;
using RedSpiderTech.Simulation.Statistics.Common.Interface;
using RedSpiderTech.Simulation.Statistics.Host.Utilities;
using Serilog;
using Serilog.Events;

namespace RedSpiderTech.Simulation.Statistics
{
    public class Program
    {
        private static IContainer _container;
        private static ILogger _logger;
        private static IAppConfigurationManager _appConfigurationManager;

        public static void Main(string[] args)
        {
            _appConfigurationManager = new AppConfigurationManager();
            string logFile = _appConfigurationManager.GetLogFile();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Verbose)
                //.WriteTo.File(logFile, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();

            _logger = Log.Logger;

            InitialiseContainer();
            
            _logger.Information("RedSpiderTech - Portfolio Statistics Calculation");
            _logger.Information("================================================");

            //var argumentParser = _container.Resolve<IArgumentParser>();
            //IStatisticsArguments statisticsArguments = argumentParser.GetStatisticsArguments(args);
            //ReportPortfolio(statisticsArguments.PortfolioReturnsFile);
            //_logger.Information("\n------------------------------------\n");
            //ReportPortfolio(statisticsArguments.BenchmarkReturnsFile);

            string[] filePaths = new string[] 
            {
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\BenchmarkUKFtse100_2012-01-01_2020-03-27.xml",
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\RebalancedUKFixedConstituents_2012-01-01_2020-03-27.xml",
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\RebalancedUKFixedConstituents_Greater_2012-01-01_2020-03-27.xml"
            };
            string[] filePaths2 = new string[]
            {
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\BenchmarkUKFtse100_2020-01-01_2020-03-27.xml",
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\RebalancedUKFixedConstituents_2020-01-01_2020-03-27.xml",
                @"C:\Users\Nikeah\Desktop\PortfolioReturns\RebalancedUKFixedConstituents_Greater_2020-01-01_2020-03-27.xml"
            };

            foreach(string filePath in filePaths2)
            {
                ReportPortfolio(filePath);
                _logger.Information("\n------------------------------------\n");
            }

            _logger.Information("Calculation completed");
        }

        private static void ReportPortfolio(string filePath)
        {
            var statisticsCalculationManager = _container.Resolve<IStatisticsCalculationManager>();
            var portfolioValuationHistoryFactory = _container.Resolve<IPortfolioValuationHistoryFactory>();
            
            IPortfolioValuationHistory portfolioValuationHistory = portfolioValuationHistoryFactory.GetPortfolioValuationHistory(filePath);
            IPortfolioStatisticsData statisticsCalculationData = statisticsCalculationManager.GetPortfolioStatisticsData(portfolioValuationHistory);

            _logger.Information($"Portfolio run from {statisticsCalculationData.StartDate.ToString("yyyy-MM-dd")} to {statisticsCalculationData.EndDate.ToString("yyyy-MM-dd")}");
            _logger.Information($"Portfolio name: {statisticsCalculationData.PortfolioIdentifier}");
            _logger.Information(string.Format("Initial valuation: {0:0.00}", statisticsCalculationData.InitialValuation));
            _logger.Information(string.Format("Final valuation: {0:0.00}", statisticsCalculationData.FinalValuation));
            _logger.Information(string.Format("Portfolio Total Returns: {0:0.00} %", statisticsCalculationData.TotalReturns));
            _logger.Information(string.Format("Portfolio Standard Deviation: {0:0.00} %", statisticsCalculationData.DailyReturnsVolatility));
            _logger.Information(string.Format("Returns to Volatility ratio: {0:0.00}", statisticsCalculationData.ReturnsToVolatilityRatio));
        }

        private static void InitialiseContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register<ILogger>(x => _logger).SingleInstance();
            containerBuilder.RegisterType<ArgumentParser>().As<IArgumentParser>();
            containerBuilder.RegisterType<PortfolioModelDeserialiser>().As<IPortfolioModelDeserialiser>();
            containerBuilder.RegisterType<StatisticsCalculationManager>().As<IStatisticsCalculationManager>();
            containerBuilder.RegisterType<PortfolioValuationHistoryFactory>().As<IPortfolioValuationHistoryFactory>();

            _container = containerBuilder.Build();
        }
    }
}
