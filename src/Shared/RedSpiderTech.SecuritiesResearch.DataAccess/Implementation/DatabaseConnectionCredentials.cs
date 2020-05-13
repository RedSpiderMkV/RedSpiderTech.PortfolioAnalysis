using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Implementation
{
    public class DatabaseConnectionCredentials : IDatabaseConnectionCredentials
    {
        #region Properties

        public string ConnectionString { get; }

        #endregion

        #region Public Methods

        public DatabaseConnectionCredentials(string username, string password, string database, string host)
        {
            ConnectionString = $"SERVER={host};DATABASE={database};UID={username};PASSWORD={password};";
        }

        #endregion
    }
}
