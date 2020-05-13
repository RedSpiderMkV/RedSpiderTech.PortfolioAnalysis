using System;
using RedSpiderTech.SecuritiesResearch.Charts.EventManager.Interface;

namespace RedSpiderTech.SecuritiesResearch.Charts.EventManager.Implementation
{
    public class ChartDataUIEventManager : IChartDataUIEventManager
    {
        #region Events

        public event EventHandler<string> NewFileSelectedEvent;

        #endregion

        #region Public Methods

        public void TriggerNewFileSelectedEvent(string selectedFile)
        {
            NewFileSelectedEvent?.Invoke(this, selectedFile);
        }

        #endregion
    }
}
