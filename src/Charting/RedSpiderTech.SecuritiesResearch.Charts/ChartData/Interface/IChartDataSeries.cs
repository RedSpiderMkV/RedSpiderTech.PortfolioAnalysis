using System.Collections.Generic;

namespace RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface
{
    public interface IChartDataSeries<T>
    {
        IEnumerable<IChartDataPoint<T>> DataPoints { get; }
        string ChartSeriesName { get; }
        int SeriesWeight { get; }

        bool InsertNewDataPoint(T xValue, decimal yValue);
    }
}