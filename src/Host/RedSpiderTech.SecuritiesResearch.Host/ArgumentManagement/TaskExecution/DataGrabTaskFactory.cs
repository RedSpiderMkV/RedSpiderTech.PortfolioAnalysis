using System;
using Autofac;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Enums;
using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model;
using Serilog;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.TaskExecution
{
    public class DataGrabTaskFactory : IDataGrabTaskFactory
    {
        private readonly ILogger _logger;

        public DataGrabTaskFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IDataGrabTask GetDataGrabTask(IContainer container, IDataArgs dataArgs)
        {
            switch (dataArgs.Task)
            {
                case TaskType.Historic:
                    return new HistoricDataGrabTask(_logger, container, dataArgs);
                case TaskType.FileBased:
                    return new FileBasedDataGrabTask(_logger, container, dataArgs);
                default:
                    throw new ArgumentException("Unsupported task type specified.");
            }
        }
    }
}
