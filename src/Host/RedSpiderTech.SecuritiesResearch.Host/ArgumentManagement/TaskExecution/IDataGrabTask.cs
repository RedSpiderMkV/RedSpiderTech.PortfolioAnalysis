using System;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution
{
    public interface IDataGrabTask : IDisposable
    {
        void Execute();
    }
}