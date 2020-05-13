using System;
using Autofac;
using RedSpiderTech.Securities.DataRetriever;
using RedSpiderTech.Securities.DataRetriever.Core;
using RedSpiderTech.SecuritiesResearch.Common.Implementation.Factory;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Factory;
using RedSpiderTech.SecuritiesResearch.DataAccess.Factories;
using RedSpiderTech.SecuritiesResearch.DataAccess.Implementation;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution;
using RedSpiderTech.SecuritiesResearch.Host.ConfigurationManagement;
using Serilog;
using Serilog.Events;

namespace RedSpiderTech.SecuritiesResearch.Host
{
    public class Program
    {
        private static IContainer _container;
        private static ILogger _logger;
        private static AppConfigurationManager _appConfigurationManager;

        public static void Main(string[] args)
        {
            Console.WriteLine("RedSpiderTech Securities Research");
            Console.WriteLine("Supported arguments:\nhistoric <symbol>\nupdate-existing\nfile-based <filePath>");

            _appConfigurationManager = new AppConfigurationManager();

            string logFile = _appConfigurationManager.GetLogFile();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Verbose)
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();

            _logger = Log.Logger;

            args = GetTestArguments();
            var argumentParser = new ArgumentParser(args);
            IDataArgs dataArgs = argumentParser.GetArgumentData();

            _logger.Information("RedSpiderTech Securities Research");
            _logger.Information("======================================");
            _logger.Information("Starting Security Data Research - Data Grab");
            _logger.Information("");

            InitialiseContainer();

            var dataGrabTaskFactory = _container.Resolve<IDataGrabTaskFactory>();
            IDataGrabTask dataGrabTask = dataGrabTaskFactory.GetDataGrabTask(_container, dataArgs);
            dataGrabTask.Execute();

            _logger.Information("");
            _logger.Information("Data Grab complete");
            _logger.Information("======================================");
        }

        private static void InitialiseContainer()
        {
            var containerBuilder = new ContainerBuilder();
            IDatabaseConnectionCredentials connectionCredentials = _appConfigurationManager.GetDatabaseConnectionCredentials();

            containerBuilder.Register<ILogger>(x => _logger).SingleInstance();
            containerBuilder.Register<IDatabaseConnectionCredentials>(x => connectionCredentials).SingleInstance();
            containerBuilder.RegisterType<MySqlConnectionWrapper>().As<IMySqlConnectionWrapper>().SingleInstance();
            containerBuilder.RegisterType<SecurityDataRetrieverManager>().As<ISecurityDataRetrieverManager>().SingleInstance();
            containerBuilder.Register(x => x.Resolve<ISecurityDataRetrieverManager>().GetSecurityDataRetriever()).As<ISecurityDataRetriever>();
            containerBuilder.RegisterType<MySqlCommandWrapperFactory>().As<IMySqlCommandWrapperFactory>();
            containerBuilder.RegisterType<PersistenceManager>().As<IPersistenceManager>();
            containerBuilder.RegisterType<DataGrabTaskFactory>().As<IDataGrabTaskFactory>();
            containerBuilder.RegisterType<DataModelFactory>().As<IDataModelFactory>();

            _container = containerBuilder.Build();
        }

        private static string[] GetTestArguments(string symbol)
        {
            return new string[] { "historic", symbol };
        }

        private static string[] GetTestArguments()
        {
            //return new string[] { "update-existing" };
            return new string[] { "file-based", @"C:\RedSpiderTech\redspidertech.securitiesresearch\ALL_SYMBOLS.txt" };
        }
    }
}
