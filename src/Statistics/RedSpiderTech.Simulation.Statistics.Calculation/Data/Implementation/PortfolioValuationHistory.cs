using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.Simulation.Statistics.Calculation.Data.Interface;

namespace RedSpiderTech.Simulation.Statistics.Calculation.Data.Implementation
{
    public class PortfolioValuationHistory : IPortfolioValuationHistory
    {
        #region Private Data

        private readonly List<IValuationData> _valuationDataCollection;

        #endregion

        #region Properties

        public string PortfolioName { get; }
        public double InitialValuation { get; }
        public DateTime StartDate => _valuationDataCollection.First().ValuationDate;
        public DateTime EndDate => _valuationDataCollection.Last().ValuationDate;
        public IEnumerable<IValuationData> ValuationData => _valuationDataCollection;

        #endregion

        #region Public Methods

        public PortfolioValuationHistory(string portfolioName, double initialValuation)
        {
            PortfolioName = portfolioName;
            InitialValuation = initialValuation;
            _valuationDataCollection = new List<IValuationData>();
        }

        public void AddValuationData(IValuationData valuationData)
        {
            _valuationDataCollection.Add(valuationData);
        }

        #endregion
    }
}
