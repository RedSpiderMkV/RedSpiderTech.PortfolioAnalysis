namespace RedSpiderTech.Simulation.Statistics.Host.Utilities
{
    public interface IArgumentParser
    {
        IStatisticsArguments GetStatisticsArguments(string[] args);
    }
}