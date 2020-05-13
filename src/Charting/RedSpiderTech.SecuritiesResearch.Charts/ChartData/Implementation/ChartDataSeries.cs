using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface;

namespace RedSpiderTech.SecuritiesResearch.Charts.ChartData.Implementation
{
    public class ChartDataSeries<T> : IChartDataSeries<T>
    {
        #region Private Data

        private readonly List<IChartDataPoint<T>> _dataPoints;

        #endregion

        #region Properties

        public IEnumerable<IChartDataPoint<T>> DataPoints => _dataPoints;
        public string ChartSeriesName { get; }
        public int SeriesWeight { get; }

        #endregion

        #region Public Methods

        public ChartDataSeries(string seriesName, IEnumerable<IChartDataPoint<T>> dataPoints, int seriesWeight)
        {
            _dataPoints = dataPoints.ToList();
            ChartSeriesName = seriesName;
            SeriesWeight = seriesWeight;
        }

        public bool InsertNewDataPoint(T xValue, decimal yValue)
        {
            if (_dataPoints.Any(x => x.XData.Equals(xValue)))
            {
                // log this somewhere perhaps
                return false;
            }

            var temp = new List<IChartDataPoint<T>>(_dataPoints);
            temp.Add(new ChartDataPoint<T>(xValue, yValue));
            IOrderedEnumerable<IChartDataPoint<T>> orderedTemp = temp.OrderBy(x => x.XData);

            _dataPoints.Clear();
            _dataPoints.AddRange(orderedTemp);
            return true;
        }

        #endregion
    }
}
