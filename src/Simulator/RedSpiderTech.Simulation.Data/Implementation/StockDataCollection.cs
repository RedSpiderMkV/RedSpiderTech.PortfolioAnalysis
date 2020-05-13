using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Data.Implementation
{
    public class StockDataWithDate : IStockDataWithDate
    {
        public IEnumerable<IStockDataModel> StockDataCollection { get; }
        public DateTime Date { get; }

        public StockDataWithDate(IEnumerable<IStockDataModel> stockDataCollection, DateTime date)
        {
            Date = date;
            StockDataCollection = stockDataCollection;
        }
    }
}
