namespace RedSpiderTech.Simulation.Simulations.Common
{
    public interface ISimulationTypeManager
    {
        ISimulationManager SimulationManager { get; }

        void InitialiseManager(string simulationType);
    }
}