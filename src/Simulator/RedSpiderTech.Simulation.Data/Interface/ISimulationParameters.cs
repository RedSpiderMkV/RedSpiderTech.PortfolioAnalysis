using System;
using System.Collections.Generic;

namespace RedSpiderTech.Simulation.Data.Interface
{
    public interface ISimulationParameters
    {
        string SimulationType { get; }
        DateTime SimulationEndDate { get; }
        DateTime SimulationStartDate { get; }
        IEnumerable<string> StockPortfolioSymbols { get; }
    }
}