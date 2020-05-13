using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface;

namespace RedSpiderTech.SecuritiesResearch.Charts.DataManager.Interface
{
    public interface IValuationDataManager : IDisposable
    {
        IEnumerable<IChartDataSeries<DateTime>> ChartDataSeriesCollection { get; }
    }
}