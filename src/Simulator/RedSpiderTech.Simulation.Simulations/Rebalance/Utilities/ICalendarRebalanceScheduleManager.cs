namespace RedSpiderTech.Simulation.Simulations.Rebalance.Utilities
{
    public interface ICalendarRebalanceScheduleManager
    {
        bool ShouldRebalance(int newMonth);
    }
}