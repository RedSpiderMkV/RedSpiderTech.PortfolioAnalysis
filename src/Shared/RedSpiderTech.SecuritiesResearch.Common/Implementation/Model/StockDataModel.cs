using System;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.SecuritiesResearch.Common.Implementation.Model
{
    public class StockDataModel : IStockDataModel
    {
        #region Properties

        public string Symbol { get; }
        public DateTime TimeStamp { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public long Volume { get; }
        public decimal AdjustedClose { get; }
        public decimal DayChange { get; }
        public decimal DayPercentageChange { get; }
        public decimal? StandardChange { get; }
        public decimal? StandardPercentageChange { get; }

        #endregion

        #region Public Methods

        public StockDataModel(string symbol,
            DateTime timeStamp,
            decimal open,
            decimal high,
            decimal low,
            decimal close,
            long volume,
            decimal adjustedClose,
            decimal dayChange,
            decimal dayPercentageChange,
            decimal standardChange,
            decimal standardPercentageChange)
        {
            Symbol = symbol;
            TimeStamp = timeStamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            AdjustedClose = adjustedClose;
            DayChange = dayChange;
            DayPercentageChange = dayPercentageChange;
            StandardChange = standardChange;
            StandardPercentageChange = standardPercentageChange;
        }

        public StockDataModel(string symbol, 
            DateTime timeStamp, 
            decimal open, 
            decimal high, 
            decimal low, 
            decimal close, 
            long volume, 
            decimal adjustedClose, 
            decimal? previousClose)
        {
            Symbol = symbol;
            TimeStamp = timeStamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            AdjustedClose = adjustedClose;
            DayChange = close - open;
            DayPercentageChange = open == 0 ? 0 : DayChange / open * 100M;
            StandardChange = previousClose == null ? null : Close - previousClose;
            StandardPercentageChange = previousClose == null ? null : StandardChange / previousClose * 100M;
        }

        public override string ToString()
        {
            return $"{Symbol}:\t{TimeStamp.ToString("yyyy-MM-dd")}\t{Open}\t{High}\t{Low}\t{Close}\t{AdjustedClose}\t{Volume}";
        }

        #endregion
    }
}
