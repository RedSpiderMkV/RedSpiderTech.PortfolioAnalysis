using System;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface IMySqlCommandWrapper : IDisposable
    {
        int ExecuteNonQuery();
        IMySqlDataReaderWrapper ExecuteReader();
        void AddParameter(string key, object value);
    }
}