using System.Configuration;
using System.IO;
using RedSpiderTech.SecuritiesResearch.DataAccess.Implementation;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Implementation;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Common.Implementation
{
    public class AppConfigurationManager : IAppConfigurationManager
    {
        public string SimulationReportDirectory => ConfigurationManager.AppSettings["simulationReportDirectory"];

        public bool UseCommandLineArgs => bool.Parse(ConfigurationManager.AppSettings["useCommandLineArgs"]);

        public string PortfolioName => ConfigurationManager.AppSettings["portfolioName"];

        public string GetLogFile()
        {
            string logFileDirectory = ConfigurationManager.AppSettings["logFileDirectory"];
            string logFileName = ConfigurationManager.AppSettings["logFileName"];
            string logFile = Path.Combine(logFileDirectory, logFileName);

            return logFile;
        }

        public IDatabaseConnectionCredentials GetDatabaseConnectionCredentials()
        {
            string userName = ConfigurationManager.AppSettings["dbUserName"];
            string password = ConfigurationManager.AppSettings["dbPassword"];
            string host = ConfigurationManager.AppSettings["dbHost"];
            string database = ConfigurationManager.AppSettings["dbDatabase"];
            var connectionCredentials = new DatabaseConnectionCredentials(userName, password, database, host);

            return connectionCredentials;
        }

        public IAccount GetAccount()
        {
            string accountName = ConfigurationManager.AppSettings["accountName"];
            decimal accountBalance = decimal.Parse(ConfigurationManager.AppSettings["accountBalance"]);
            string accountCurrency = ConfigurationManager.AppSettings["accountCurrency"];
            decimal commission = decimal.Parse(ConfigurationManager.AppSettings["transactionCommission"]);

            return new Account(accountName, accountCurrency, accountBalance, commission);
        }
    }
}
