using System;
using System.Collections.Generic;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Data.Implementation
{
    public class SimulationParameters : ISimulationParameters
    {
        public string SimulationType { get; }
        public DateTime SimulationStartDate { get; }
        public DateTime SimulationEndDate { get; }
        public IEnumerable<string> StockPortfolioSymbols { get; }

        public SimulationParameters(string simulationType, DateTime startDate, DateTime endDate, IEnumerable<string> stockSymbols)
        {
            SimulationType = simulationType;
            SimulationStartDate = startDate;
            SimulationEndDate = endDate;
            StockPortfolioSymbols = stockSymbols;
        }
    }
}
