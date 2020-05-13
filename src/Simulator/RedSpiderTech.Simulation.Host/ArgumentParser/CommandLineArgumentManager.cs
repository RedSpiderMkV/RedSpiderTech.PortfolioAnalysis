using System;
using System.Collections.Generic;
using RedSpiderTech.Simulation.Data.Implementation;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Host.ArgumentParser
{
    public class CommandLineArgumentManager
    {
        #region Private Data

        private readonly string[] _args;

        #endregion

        #region Public Methods

        public CommandLineArgumentManager(bool usingCommandLine, string[] args)
        {
            if(!usingCommandLine)
            {
                return;
            }

            if (args.Length != 4)
            {
                Console.WriteLine("Invalid command line arguments");
                Console.WriteLine("<simulation> <startDate> <endDate> <commandSeparatedSymbols>");
                throw new InvalidProgramException("Unable to proceed with simulation");
            }

            _args = args;
        }

        public string GetSimulationType()
        {
            return _args[0].ToLower();
        }

        public ISimulationParameters GetSimulationParameters()
        {
            string simulationType = GetSimulationType();
            DateTime startDate = DateTime.Parse(_args[1]);
            DateTime endDate = DateTime.Parse(_args[2]);
            IEnumerable<string> symbolCollection = _args[3].Split(',');

            return new SimulationParameters(simulationType, startDate, endDate, symbolCollection);
        }

        #endregion
    }
}
