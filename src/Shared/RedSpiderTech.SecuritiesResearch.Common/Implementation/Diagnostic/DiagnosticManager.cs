using System;
using System.Diagnostics;
using Serilog;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic;

namespace RedSpiderTech.SecuritiesResearch.Common.Implementation.Diagnostic
{
    public class DiagnosticManager : IDiagnosticManager
    {
        #region Private Methods

        private readonly ILogger _logger;

        #endregion

        #region Public Methods

        public DiagnosticManager(ILogger logger)
        {
            _logger = logger;

            _logger.Information("DiagnosticManager: Instantiation successful.");
        }

        public void LogRunTime(Action action)
        {
            Stopwatch watch = Stopwatch.StartNew();
            action();
            watch.Stop();

            _logger.Information($"Rebalancing run time: {watch.ElapsedTicks}");
        }

        #endregion
    }
}
