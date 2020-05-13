using System;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement
{
    public class ArgumentParser
    {
        #region Private Data

        private readonly string[] _arguments;

        #endregion

        #region Public Methods

        public ArgumentParser(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new ArgumentException("Invalid command line arguments.");
            }

            _arguments = arguments;
        }

        public IDataArgs GetArgumentData()
        {
            if (_arguments[0].Equals("historic"))
            {
                return new HistoricDataArgs(GetData());
            }

            if(_arguments[0].Equals("file-based"))
            {
                return new FileBaseDataArgs(GetData());
            }

            throw new ArgumentException("Directive not recognised.");
        }

        #endregion

        private string GetData()
        {
            if (_arguments.Length != 2)
            {
                throw new ArgumentException("Invalid arg count.");
            }

            string commandLineArgument = _arguments[1];
            return commandLineArgument;
        }
    }
}
