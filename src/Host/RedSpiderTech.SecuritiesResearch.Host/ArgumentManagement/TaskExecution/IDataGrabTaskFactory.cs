using Autofac;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution
{
    public interface IDataGrabTaskFactory
    {
        IDataGrabTask GetDataGrabTask(IContainer container, IDataArgs dataArgs);
    }
}