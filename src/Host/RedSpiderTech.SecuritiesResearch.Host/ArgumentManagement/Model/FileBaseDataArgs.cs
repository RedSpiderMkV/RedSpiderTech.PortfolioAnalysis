using RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Enums;

namespace RedSpiderTech.SecuritiesResearch.Host.ArgumentManagement.Model
{
    public class FileBaseDataArgs : IDataArgs
    {
        #region Properties

        public string FilePath { get; }
        public TaskType Task => TaskType.FileBased;

        #endregion

        #region Public Methods

        public FileBaseDataArgs(string filePath)
        {
            FilePath = filePath;
        }

        #endregion
    }
}
