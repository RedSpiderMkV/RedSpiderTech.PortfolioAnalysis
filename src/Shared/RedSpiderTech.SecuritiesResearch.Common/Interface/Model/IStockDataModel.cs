using System;

namespace RedSpiderTech.SecuritiesResearch.Common.Interface.Model
{
    public interface IStockDataModel
    {
        string Symbol { get; }
        decimal AdjustedClose { get; }
        decimal Close { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Open { get; }
        DateTime TimeStamp { get; }
        long Volume { get; }
        decimal DayChange { get; }
        decimal DayPercentageChange { get; }
        decimal? StandardChange { get; }
        decimal? StandardPercentageChange { get; }
    }
}