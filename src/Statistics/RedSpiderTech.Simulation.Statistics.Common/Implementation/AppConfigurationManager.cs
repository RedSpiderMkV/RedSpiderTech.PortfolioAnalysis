using System.Configuration;
using System.IO;
using RedSpiderTech.Simulation.Statistics.Common.Interface;

namespace RedSpiderTech.Simulation.Statistics.Common.Implementation
{
    public class AppConfigurationManager : IAppConfigurationManager
    {
        public string GetLogFile()
        {
            string logFileDirectory = ConfigurationManager.AppSettings["logFileDirectory"];
            string logFileName = ConfigurationManager.AppSettings["logFileName"];
            string logFile = Path.Combine(logFileDirectory, logFileName);

            return logFile;
        }
    }
}
