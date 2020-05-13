using System;
using Autofac;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.DataManager.Interface;
using RedSpiderTech.Simulation.Simulations.BuyAndHold;
using RedSpiderTech.Simulation.Simulations.Rebalance;
using RedSpiderTech.Simulation.Simulations.Rebalance.Utilities;
using Serilog;

namespace RedSpiderTech.Simulation.Simulations.Common
{
    public class SimulationTypeManager : ISimulationTypeManager
    {
        #region Properties

        public ISimulationManager SimulationManager { get; private set; }

        #endregion

        #region Private Data

        private readonly IAccount _account;
        private readonly IMarketDataManager _marketDataManager;
        private readonly INotificationManager _notificationManager;
        private readonly ILogger _logger;
        private readonly ICalendarRebalanceScheduleManager _calendarRebalanceScheduleManager;
        private readonly IDiagnosticManager _diagnosticManager;

        private readonly IContainer _container;

        #endregion

        #region Public Methods

        public SimulationTypeManager(ILogger logger,
                                     IAppConfigurationManager configurationManager,
                                     IDiagnosticManager diagnosticManager,
                                     IMarketDataManager marketDataManager,
                                     INotificationManager notificationManager,
                                     ICalendarRebalanceScheduleManager rebalanceScheduleManager)
        {
            _account = configurationManager.GetAccount();
            _logger = logger;
            _diagnosticManager = diagnosticManager;
            _notificationManager = notificationManager;
            _marketDataManager = marketDataManager;
            _calendarRebalanceScheduleManager = rebalanceScheduleManager;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register<IDiagnosticManager>(x => _diagnosticManager).SingleInstance();
            containerBuilder.Register<IMarketDataManager>(x => _marketDataManager).SingleInstance();
            containerBuilder.Register<INotificationManager>(x => _notificationManager).SingleInstance();
            containerBuilder.Register<ICalendarRebalanceScheduleManager>(x => _calendarRebalanceScheduleManager).SingleInstance();
            containerBuilder.Register<IAppConfigurationManager>(x => configurationManager).SingleInstance();
            containerBuilder.Register<ILogger>(x => _logger).SingleInstance();

            containerBuilder.RegisterType<RebalanceSimulationManager>().As<ISimulationManager, IRebalanceSimulationManager>().SingleInstance();
            containerBuilder.RegisterType<BuyAndHoldSimulationManager>().As<ISimulationManager, IBuyAndHoldSimulationManager>().SingleInstance();

            _container = containerBuilder.Build();
        }

        public void InitialiseManager(string simulationType)
        {
            switch (simulationType.ToLower())
            {
                case "rebalance":
                    SimulationManager = _container.Resolve<IRebalanceSimulationManager>();
                    break;
                case "buyandhold":
                    SimulationManager = _container.Resolve<IBuyAndHoldSimulationManager>();
                    break;
                default:
                    throw new NotImplementedException("Unknown simulation type requested.");
            }
        }

        #endregion
    }
}
