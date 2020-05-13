using MySql.Data.MySqlClient;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers
{
    public class MySqlDataReaderWrapper : IMySqlDataReaderWrapper
    {
        #region Properties

        public bool HasRows { get { return _dataReader.HasRows; } }

        #endregion

        #region Private Data

        private readonly MySqlDataReader _dataReader;

        #endregion

        #region Public Methods

        public MySqlDataReaderWrapper(MySqlDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        public bool ReadNext()
        {
            return _dataReader.Read();
        }

        public T GetField<T>(string fieldName)
        {
            return (T)_dataReader[fieldName];
        }

        public void Dispose()
        {
            _dataReader.Dispose();
        }

        #endregion
    }
}
