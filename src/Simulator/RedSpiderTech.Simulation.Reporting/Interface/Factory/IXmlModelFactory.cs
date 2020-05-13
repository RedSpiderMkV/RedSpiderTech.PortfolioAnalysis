using System;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Reporting.Interface.Factory
{
    public interface IXmlModelFactory
    {
        PortfolioValuationDataReportingModel GetValuationDataReportingModel(DateTime valuationDate, IAccount account);
        PortfolioValuationSummaryDataModel GetValuationSummaryDataModel();
        PortfolioRunStatistics GetPortfolioRunStatistics(IPortfolioStatisticsData statisticsData);
    }
}