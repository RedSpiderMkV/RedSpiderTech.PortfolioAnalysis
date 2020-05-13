using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;

namespace RedSpiderTech.Simulation.Data.Interface
{
    public interface IAccount
    {
        /// <summary>
        /// Account balance.
        /// </summary>
        decimal Balance { get; }

        /// <summary>
        /// Account currency.
        /// </summary>
        string Currency { get; }

        /// <summary>
        /// Account name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Account holdings.
        /// </summary>
        Dictionary<string, IHolding> AccountHoldings { get; }

        /// <summary>
        /// Current account valuation.
        /// </summary>
        decimal CurrentValuation { get; }

        /// <summary>
        /// Get the current commission charge per transaction.
        /// </summary>
        decimal Commission { get; }

        bool CanBuy(uint quantity, decimal price);
        void Buy(string symbol, uint quantity, decimal price);
        bool CanSell(uint quantity, string symbol);
        void Sell(string symbol, uint quantity);
        void UpdateHolding(IStockDataModel stockData);
        decimal GetAvailableBalanceForBuying(uint symbolCount);
    }
}