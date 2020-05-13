using System;
using System.Windows.Forms;
using RedSpiderTech.SecuritiesResearch.Charts.DataManager.Implementation;
using RedSpiderTech.SecuritiesResearch.Charts.EventManager.Implementation;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Implementation;
using Serilog;
using Serilog.Events;

namespace RedSpiderTech.SecuritiesResearch.Charts
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Verbose)
                .CreateLogger();

            var chartDataUIEventManager = new ChartDataUIEventManager();
            var portfolioModelDeserialiser = new PortfolioModelDeserialiser(Log.Logger);
            var valuationDataManager = new ValuationDataManager(chartDataUIEventManager, portfolioModelDeserialiser);

            Application.Run(new Form1(chartDataUIEventManager, valuationDataManager));
        }
    }
}
