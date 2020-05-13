using System; 
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.Reporting.Interface.Factory;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;
using Serilog;

namespace RedSpiderTech.Simulation.Reporting.Implementation.Factory
{
    public class XmlModelFactory : IXmlModelFactory
    {
        #region Private Data

        private readonly ILogger _logger;
        private readonly IAppConfigurationManager _appConfigurationManager;

        #endregion

        #region Public Methods

        public XmlModelFactory(ILogger logger, IAppConfigurationManager appConfigurationManager)
        {
            _appConfigurationManager = appConfigurationManager;
            _logger = logger;
        }

        public PortfolioValuationDataReportingModel GetValuationDataReportingModel(DateTime valuationDate, IAccount account)
        {
            var holdingValuations = new List<HoldingValuationDataReportingModel>();
            foreach (KeyValuePair<string, IHolding> holding in account.AccountHoldings)
            {
                decimal valuation = holding.Value.CurrentPrice * holding.Value.Quantity;
                var holdingValuation = new HoldingValuationDataReportingModel { StockSymbol = holding.Key, HoldingValue = valuation };
                holdingValuations.Add(holdingValuation);
            }

            var portfolioValuation = new PortfolioValuationDataReportingModel
            {
                ValuationDate = valuationDate,
                PortfolioValuation = account.CurrentValuation,
                HoldingValuations = holdingValuations.ToArray()
            };

            return portfolioValuation;
        }

        public PortfolioValuationSummaryDataModel GetValuationSummaryDataModel()
        {
            PortfolioMetaData portfolioMetaData = GetPortfolioMetaData();
            var dataModel = new PortfolioValuationSummaryDataModel { MetaData = portfolioMetaData };
            return dataModel;
        }

        public PortfolioRunStatistics GetPortfolioRunStatistics(IPortfolioStatisticsData statisticsData)
        {
            var portfolioRunStatistics = new PortfolioRunStatistics
            {
                DailyReturnsVolatility = statisticsData.DailyReturnsVolatility,
                FinalValuation = statisticsData.FinalValuation,
                InitialValuation = statisticsData.InitialValuation,
                ReturnsToVolatilityRatio = statisticsData.ReturnsToVolatilityRatio,
                TotalReturns = statisticsData.TotalReturns
            };

            return portfolioRunStatistics;
        }

        #endregion

        #region Private Methods

        private PortfolioMetaData GetPortfolioMetaData()
        {
            IAccount accountData = _appConfigurationManager.GetAccount();
            string portfolioName = _appConfigurationManager.PortfolioName;
            var portfolioMetaData = new PortfolioMetaData
            {
                PortfolioName = portfolioName,
                RebalanceEnabled = true,
                InitialBalance = accountData.Balance,
                Currency = accountData.Currency
            };

            return portfolioMetaData;
        }

        #endregion
    }
}
