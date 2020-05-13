using System;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation
{
    public class ValuationData : IValuationData
    {
        #region Properties

        public DateTime ValuationDate { get; }
        public double Valuation { get; }

        #endregion

        #region

        public ValuationData(DateTime valuationDate, double valuation)
        {
            ValuationDate = valuationDate;
            Valuation = valuation;
        }

        #endregion
    }
}
