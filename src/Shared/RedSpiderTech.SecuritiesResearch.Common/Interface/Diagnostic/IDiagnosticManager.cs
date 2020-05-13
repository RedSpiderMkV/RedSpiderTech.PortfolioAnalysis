using System;

namespace RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic
{
    public interface IDiagnosticManager
    {
        void LogRunTime(Action action);
    }
}