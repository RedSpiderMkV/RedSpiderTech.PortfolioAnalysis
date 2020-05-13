using System;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface
{
    public interface IValuationData
    {
        double Valuation { get; }
        DateTime ValuationDate { get; }
    }
}