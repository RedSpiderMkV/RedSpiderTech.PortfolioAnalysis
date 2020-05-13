using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Common.Interface
{
    public interface IAppConfigurationManager
    {
        bool UseCommandLineArgs { get; }
        string SimulationReportDirectory { get; }
        string PortfolioName { get; }

        IAccount GetAccount();
        IDatabaseConnectionCredentials GetDatabaseConnectionCredentials();
        string GetLogFile();
    }
}