using System;
using MySql.Data.MySqlClient;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface IMySqlConnectionWrapper : IDisposable
    {
        MySqlConnection DatabaseConnection { get; }

        void Open();
        void Close();
    }
}