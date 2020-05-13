using MySql.Data.MySqlClient;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers
{
    public class MySqlCommandWrapper : IMySqlCommandWrapper
    {
        #region Private Data

        private readonly MySqlCommand _command;

        #endregion

        #region Public Methods

        public MySqlCommandWrapper(string commandString, IMySqlConnectionWrapper connectionWrapper)
        {
            _command = new MySqlCommand(commandString, connectionWrapper.DatabaseConnection);
        }

        public void AddParameter(string key, object value)
        {
            _command.Parameters.AddWithValue(key, value);
        }

        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        public IMySqlDataReaderWrapper ExecuteReader()
        {
            return new MySqlDataReaderWrapper(_command.ExecuteReader());
        }

        public void Dispose()
        {
            _command.Dispose();
        }

        #endregion
    }
}
