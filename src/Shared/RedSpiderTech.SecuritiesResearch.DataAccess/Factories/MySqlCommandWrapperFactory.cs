using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Factories
{
    public class MySqlCommandWrapperFactory : IMySqlCommandWrapperFactory
    {
        #region Public Methods

        public IMySqlCommandWrapper GetCommandWrapper(string commandString, IMySqlConnectionWrapper connectionWrapper)
        {
            return new MySqlCommandWrapper(commandString, connectionWrapper);
        }

        #endregion
    }
}
