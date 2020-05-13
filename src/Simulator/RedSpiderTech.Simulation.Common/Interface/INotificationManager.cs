using System;

namespace RedSpiderTech.Simulation.Common.Interface
{
    public interface INotificationManager
    {
        event EventHandler SimulationEnded;
        event EventHandler SimulationStarted;

        void TriggerSimulationEnded();
        void TriggerSimulationStart();
    }
}