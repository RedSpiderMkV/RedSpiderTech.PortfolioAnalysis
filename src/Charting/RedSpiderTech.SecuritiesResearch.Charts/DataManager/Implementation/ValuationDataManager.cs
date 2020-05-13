using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Implementation;
using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface;
using RedSpiderTech.SecuritiesResearch.Charts.DataManager.Interface;
using RedSpiderTech.SecuritiesResearch.Charts.EventManager.Interface;
using RedSpiderTech.SecuritiesResearch.XMLReport.Factory.Interface;
using RedSpiderTech.SecuritiesResearch.XMLReport.Model;

namespace RedSpiderTech.SecuritiesResearch.Charts.DataManager.Implementation
{
    public class ValuationDataManager : IValuationDataManager
    {
        #region Private Data

        private readonly IPortfolioModelDeserialiser _portfolioModelDeserialiser;
        private readonly IChartDataUIEventManager _chartDataUIEventManager;
        private readonly List<IChartDataSeries<DateTime>> _chartDataSeriesCollection;

        #endregion

        #region Properties

        public IEnumerable<IChartDataSeries<DateTime>> ChartDataSeriesCollection => _chartDataSeriesCollection;

        #endregion

        #region Public Methods

        public ValuationDataManager(IChartDataUIEventManager chartDataUIEventManager, IPortfolioModelDeserialiser portfolioModelDeserialiser)
        {
            _chartDataUIEventManager = chartDataUIEventManager;
            _portfolioModelDeserialiser = portfolioModelDeserialiser;
            _chartDataSeriesCollection = new List<IChartDataSeries<DateTime>>();

            _chartDataUIEventManager.NewFileSelectedEvent += ChartDataUIEventManager_NewFileSelectedEvent;
        }

        public void Dispose()
        {
            _chartDataUIEventManager.NewFileSelectedEvent -= ChartDataUIEventManager_NewFileSelectedEvent;
        }

        #endregion

        #region Private Methods

        private void ChartDataUIEventManager_NewFileSelectedEvent(object sender, string reportXmlFile)
        {
            PortfolioValuationSummaryDataModel portfolioValuationSummaryDataModel = _portfolioModelDeserialiser.GetReportDataModel(reportXmlFile);

            var holdingsValuation = new Dictionary<string, List<IChartDataPoint<DateTime>>>();
            foreach (PortfolioValuationDataReportingModel reportingPoint in portfolioValuationSummaryDataModel.PortfolioValuationData)
            {
                foreach (HoldingValuationDataReportingModel holdingsDataPoint in reportingPoint.HoldingValuations)
                {
                    if (!holdingsValuation.ContainsKey(holdingsDataPoint.StockSymbol))
                    {
                        holdingsValuation.Add(holdingsDataPoint.StockSymbol, new List<IChartDataPoint<DateTime>>());
                    }

                    holdingsValuation[holdingsDataPoint.StockSymbol].Add(new ChartDataPoint<DateTime>(reportingPoint.ValuationDate, holdingsDataPoint.HoldingValue));
                }
            }

            IEnumerable<ChartDataSeries<DateTime>> holdingsDataSeries = holdingsValuation.Select(x =>
            {
                return new ChartDataSeries<DateTime>(x.Key, x.Value, 1);
            });

            _chartDataSeriesCollection.Clear();
            _chartDataSeriesCollection.Add(GetPortfolioValuation(portfolioValuationSummaryDataModel));
            _chartDataSeriesCollection.AddRange(holdingsDataSeries);

            IEnumerable<DateTime> valuationDates = portfolioValuationSummaryDataModel.PortfolioValuationData.Select(x => x.ValuationDate);
            _chartDataSeriesCollection.ForEach(x => FillNa(x, valuationDates, 0));
        }

        private void FillNa(IChartDataSeries<DateTime> dataSeries, IEnumerable<DateTime> valuationDates, decimal fillValue)
        {
            foreach(DateTime date in valuationDates)
            {
                if(dataSeries.DataPoints.Any(x => x.XData == date))
                {
                    continue;
                }

                dataSeries.InsertNewDataPoint(date, fillValue);
            }
        }

        private static IChartDataSeries<DateTime> GetPortfolioValuation(PortfolioValuationSummaryDataModel portfolioValuationSummaryDataModel)
        {
            IEnumerable<ChartDataPoint<DateTime>> portfolioDataPoints = portfolioValuationSummaryDataModel.PortfolioValuationData.Select(x =>
            {
                return new ChartDataPoint<DateTime>(x.ValuationDate, x.PortfolioValuation);
            });

            var portfolioValuationSeries = new ChartDataSeries<DateTime>("Portfolio Valuation", portfolioDataPoints, 3);
            return portfolioValuationSeries;
        }

        #endregion
    }
}
