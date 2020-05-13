using RedSpiderTech.SecuritiesResearch.XMLReport.Model;

namespace RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Interface
{
    public interface IPortfolioModelDeserialiser
    {
        PortfolioValuationSummaryDataModel GetReportDataModel(string xmlReportFile);
    }
}