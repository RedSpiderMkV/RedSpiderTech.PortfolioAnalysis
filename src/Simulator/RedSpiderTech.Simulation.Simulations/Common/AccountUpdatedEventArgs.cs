using System;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Simulations.Common
{
    public class AccountUpdatedEventArgs : EventArgs
    {
        #region Properties

        public IAccount Account { get; }
        public DateTime ValuationDate { get; }

        #endregion

        #region Public Methods

        public AccountUpdatedEventArgs(IAccount account, DateTime valuationDate)
        {
            Account = account;
            ValuationDate = valuationDate;
        }

        #endregion
    }
}
