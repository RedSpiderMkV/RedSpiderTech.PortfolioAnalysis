using System;

namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface IMySqlDataReaderWrapper : IDisposable
    {
        bool HasRows { get; }

        bool ReadNext();

        T GetField<T>(string fieldName);
    }
}