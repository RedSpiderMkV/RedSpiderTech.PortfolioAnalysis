using System.Configuration;
using System.IO;
using RedSpiderTech.SecuritiesResearch.DataAccess.Implementation;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.Host.ConfigurationManagement
{
    public class AppConfigurationManager
    {
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
    }
}
