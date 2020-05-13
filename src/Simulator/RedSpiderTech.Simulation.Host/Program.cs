using Autofac;
using RedSpiderTech.SecuritiesResearch.Common.Implementation.Diagnostic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic;
using RedSpiderTech.SecuritiesResearch.DataAccess.Factories;
using RedSpiderTech.SecuritiesResearch.DataAccess.Implementation;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers;
using RedSpiderTech.Simulation.Common.Implementation;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.DataManager.Implementation;
using RedSpiderTech.Simulation.DataManager.Interface;
using RedSpiderTech.Simulation.Host.ArgumentParser;
using RedSpiderTech.Simulation.Reporting.Implementation;
using RedSpiderTech.Simulation.Reporting.Implementation.Factory;
using RedSpiderTech.Simulation.Reporting.Implementation.Utilities;
using RedSpiderTech.Simulation.Reporting.Interface;
using RedSpiderTech.Simulation.Reporting.Interface.Factory;
using RedSpiderTech.Simulation.Reporting.Interface.Utilities;
using RedSpiderTech.Simulation.Simulations.BuyAndHold;
using RedSpiderTech.Simulation.Simulations.Common;
using RedSpiderTech.Simulation.Simulations.Rebalance;
using RedSpiderTech.Simulation.Simulations.Rebalance.Utilities;
using RedSpiderTech.Simulation.Statistics.Calculation;
using Serilog;
using Serilog.Events;

namespace RedSpiderTech.Simulation.Host
{
    internal class Program
    {
        private static IContainer _container;
        private static ILogger _logger;
        private static bool _simulationComplete = false;
        private static IAppConfigurationManager _appConfigurationManager;
        private static CommandLineArgumentManager _commandLineArgumentManager;

        internal static void Main(string[] args)
        {
            _appConfigurationManager = new AppConfigurationManager();
            _commandLineArgumentManager = new CommandLineArgumentManager(_appConfigurationManager.UseCommandLineArgs, args);

            string logFile = _appConfigurationManager.GetLogFile();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Verbose)
                //.WriteTo.File(logFile, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();

            _logger = Log.Logger;

            InitialiseContainer();
            ISimulationTypeManager simulationTypeManager = _container.Resolve<ISimulationTypeManager>();
            simulationTypeManager.InitialiseManager(_commandLineArgumentManager.GetSimulationType());

            if(simulationTypeManager.SimulationManager == null)
            {
                _logger.Fatal("Unknown simulation type supplied.  Unable to proceed...");
                return;
            }

            simulationTypeManager.SimulationManager.Start();

            IAccount primaryAccount = _appConfigurationManager.GetAccount();
            ISimulationParameters simulationParameters = _container.Resolve<ISimulationParameters>();

            _logger.Information("Portfolio Simulation");
            _logger.Information("====================");
            _logger.Information($"Initial account: Currency: {primaryAccount.Currency} Balance: {primaryAccount.Balance}");
            _logger.Information($"Simulation Period: {simulationParameters.SimulationStartDate.ToShortDateString()} - {simulationParameters.SimulationEndDate.ToShortDateString()}");
            _logger.Information($"Simulation Type: {simulationParameters.SimulationType}");

            _logger.Information("Simulation starting...");
            var notificationManager = _container.Resolve<INotificationManager>();

            notificationManager.SimulationEnded += (s, e) => _simulationComplete = true;
            notificationManager.TriggerSimulationStart();

            while(!_simulationComplete)
            {
                System.Threading.Thread.Sleep(1000 * 10); // sleep for 10 seconds
            }
        }

        private static void InitialiseContainer()
        {
            var containerBuilder = new ContainerBuilder();
            IDatabaseConnectionCredentials connectionCredentials = _appConfigurationManager.GetDatabaseConnectionCredentials();
            ISimulationParameters simulationParameters = _commandLineArgumentManager.GetSimulationParameters();

            containerBuilder.Register<ILogger>(x => _logger).SingleInstance();
            containerBuilder.Register<IDatabaseConnectionCredentials>(x => connectionCredentials).SingleInstance();
            containerBuilder.Register<ISimulationParameters>(x => simulationParameters).SingleInstance();
            containerBuilder.Register<IAppConfigurationManager>(x => _appConfigurationManager).SingleInstance();
            
            containerBuilder.RegisterType<SimulationTypeManager>().As<ISimulationTypeManager>().SingleInstance();
            containerBuilder.RegisterType<MySqlConnectionWrapper>().As<IMySqlConnectionWrapper>().SingleInstance();
            containerBuilder.RegisterType<PersistenceManager>().As<IPersistenceManager>().SingleInstance();
            containerBuilder.RegisterType<SecurityDataReader>().As<ISecurityDataReader>().SingleInstance();
            containerBuilder.RegisterType<NotificationManager>().As<INotificationManager>().SingleInstance();
            containerBuilder.RegisterType<DiagnosticManager>().As<IDiagnosticManager>().SingleInstance();

            containerBuilder.RegisterType<CalendarRebalanceScheduleManager>().As<ICalendarRebalanceScheduleManager>();
            containerBuilder.RegisterType<SecurityDataSqlFactory>().As<ISecurityDataSqlFactory>();
            containerBuilder.RegisterType<MySqlCommandWrapperFactory>().As<IMySqlCommandWrapperFactory>();
            containerBuilder.RegisterType<XmlModelFactory>().As<IXmlModelFactory>();
            containerBuilder.RegisterType<PortfolioValuationSummaryXmlWriter>().As<IPortfolioValuationSummaryReportWriter>();
            containerBuilder.RegisterType<StatisticsCalculationManager>().As<IStatisticsCalculationManager>();
            containerBuilder.RegisterType<HashingFilenameGenerator>().As<IHashingFilenameGenerator>();

            containerBuilder.RegisterType<PortfolioValuationReporter>().As<IPortfolioValuationReporter, IStartable>().SingleInstance();
            containerBuilder.RegisterType<MarketDataManager>().As<IMarketDataManager, IStartable>().SingleInstance();

            _container = containerBuilder.Build();
        }
    }
}
