using System;
using System.Collections.Generic;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface
{
    public interface IPortfolioValuationHistory
    {
        double InitialValuation { get; }
        IEnumerable<IValuationData> ValuationData { get; }
        string PortfolioName { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }

        void AddValuationData(IValuationData valuationData);
    }
}