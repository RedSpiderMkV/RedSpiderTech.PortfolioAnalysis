using System;

namespace RedSpiderTech.SecuritiesResearch.Charts.EventManager.Interface
{
    public interface IChartDataUIEventManager
    {
        event EventHandler<string> NewFileSelectedEvent;

        void TriggerNewFileSelectedEvent(string selectedFile);
    }
}