using MySql.Data.MySqlClient;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Wrappers
{
    public class MySqlConnectionWrapper : IMySqlConnectionWrapper
    {
        #region Properties

        public MySqlConnection DatabaseConnection { get; }

        #endregion

        #region Public Methods

        public MySqlConnectionWrapper(IDatabaseConnectionCredentials connectionCredentials)
        {
            DatabaseConnection = new MySqlConnection(connectionCredentials.ConnectionString);
        }

        public void Open()
        {
            DatabaseConnection.Open();
        }

        public void Close()
        {
            DatabaseConnection.Close();
        }

        public void Dispose()
        {
            DatabaseConnection.Dispose();
        }

        #endregion
    }
}
