using System;
using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.Simulation.Data.Interface
{
    public interface IStockDataWithDate
    {
        DateTime Date { get; }
        IEnumerable<IStockDataModel> StockDataCollection { get; }
    }
}