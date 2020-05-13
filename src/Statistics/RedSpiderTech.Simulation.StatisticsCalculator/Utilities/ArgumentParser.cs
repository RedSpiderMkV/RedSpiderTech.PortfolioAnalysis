using System;
using System.IO;

namespace RedSpiderTech.Simulation.Statistics.Host.Utilities
{
    public class ArgumentParser : IArgumentParser
    {
        #region Public Methods

        public IStatisticsArguments GetStatisticsArguments(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Invalid number of arguments.  Expected two arguments, a path to a portfolio returns file and a benchmark returns file.");
            }

            ValidateInputFile(args[0], "portfolio returns");
            ValidateInputFile(args[1], "benchmark returns");

            return new StatisticsArguments(args[0], args[1]);
        }

        #endregion

        #region Private Methods

        private static void ValidateInputFile(string filePath, string fileType)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"Specified {fileType} file does not exist");
            }
        }

        #endregion
    }
}
