using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Enums;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model
{
    public class HistoricDataArgs : IDataArgs
    {
        #region Properties

        public IReadOnlyDictionary<DataGrabParameters, string> Parameters { get; }
        public TaskType Task => TaskType.Historic;

        #endregion

        #region Public Methods

        public HistoricDataArgs(string symbol)
        {
            Parameters = new Dictionary<DataGrabParameters, string>
            {
                { DataGrabParameters.Symbol, symbol }
            };
        }

        #endregion
    }
}
