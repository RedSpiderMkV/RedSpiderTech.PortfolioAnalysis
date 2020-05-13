using RedSpiderTech.SecuritiesResearch.XMLReport.Model;

namespace RedSpiderTech.Simulation.Reporting.Interface.Utilities
{
    public interface IPortfolioValuationSummaryReportWriter
    {
        void Write(PortfolioValuationSummaryDataModel portfolioValuationSummary);
    }
}