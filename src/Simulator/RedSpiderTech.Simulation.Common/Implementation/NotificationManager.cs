using System;
using RedSpiderTech.Simulation.Common.Interface;
using Serilog;

namespace RedSpiderTech.Simulation.Common.Implementation
{
    public class NotificationManager : INotificationManager
    {
        #region Events

        public event EventHandler SimulationStarted;
        public event EventHandler SimulationEnded;

        #endregion

        #region Private Data

        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public NotificationManager(ILogger logger)
        {
            _logger = logger;
        }

        public void TriggerSimulationStart()
        {
            _logger.Information("NotificationManager: Simulation start triggered.");

            SimulationStarted?.Invoke(this, null);
        }

        public void TriggerSimulationEnded()
        {
            _logger.Information("NotificationManager: Simulation end triggered.");

            SimulationEnded?.Invoke(this, null);
        }

        #endregion
    }
}
