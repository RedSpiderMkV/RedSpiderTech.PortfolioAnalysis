using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.Reporting.Interface.Utilities;
using Serilog;

namespace RedSpiderTech.Simulation.Reporting.Implementation.Utilities
{
    public class HashingFilenameGenerator : IHashingFilenameGenerator
    {
        #region Private Data

        private readonly ISimulationParameters _simulationParameters;
        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public HashingFilenameGenerator(ILogger logger, ISimulationParameters simulationParameters)
        {
            _simulationParameters = simulationParameters;
            _logger = logger;
        }

        public string GetFilename()
        {
            IOrderedEnumerable<string> orderedSymbols = _simulationParameters.StockPortfolioSymbols.OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase);
            string allSymbols = string.Join(",", orderedSymbols.ToArray());

            string hashedValue = CreateMD5(allSymbols);
            string filename = string.Format("{0}_{1}_{2}-{3}.xml",
                                            hashedValue,
                                            _simulationParameters.SimulationStartDate.ToString("yyyy-MM-dd"),
                                            _simulationParameters.SimulationEndDate.ToString("yyyy-MM-dd"),
                                            _simulationParameters.SimulationType);

            _logger.Information($"HashingFilenameGenerator: Generated filename: {filename}");
            return filename;
        }

        #endregion

        #region Private Methods

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        #endregion
    }
}
