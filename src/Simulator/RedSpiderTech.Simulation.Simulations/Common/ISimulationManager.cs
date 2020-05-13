using System;
using RedSpiderTech.Simulation.Simulations.Rebalance;

namespace RedSpiderTech.Simulation.Simulations.Common
{
    public interface ISimulationManager : IDisposable
    {
        event EventHandler<AccountUpdatedEventArgs> AccountUpdatedEvent;

        void Start();
    }
}
