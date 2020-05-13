using System.Collections.Generic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Data.Implementation
{
    public class Account : IAccount
    {
        #region Properties

        /// <inheritdoc />
        public string Name { get; }
        /// <inheritdoc />
        public decimal Balance { get; private set; }
        /// <inheritdoc />
        public string Currency { get; }
        /// <inheritdoc />
        public decimal Commission { get; }
        /// <inheritdoc />
        public Dictionary<string, IHolding> AccountHoldings { get; }
        /// <inheritdoc />
        public decimal CurrentValuation
        {
            get
            {
                decimal currentValuation = Balance;
                foreach (KeyValuePair<string, IHolding> holding in AccountHoldings)
                {
                    currentValuation += holding.Value.CurrentPrice * holding.Value.Quantity;
                }

                return currentValuation;
            }
        }

        #endregion

        #region Public Methods

        public Account(string name, string currency, decimal initialBalance, decimal commission)
        {
            Name = name;
            Balance = initialBalance;
            Currency = currency;
            Commission = commission;

            AccountHoldings = new Dictionary<string, IHolding>();
        }

        public decimal GetAvailableBalanceForBuying(uint symbolCount)
        {
            return Balance - (Commission * symbolCount);
        }

        public bool CanBuy(uint quantity, decimal price)
        {
            decimal transactionCost = GetTransactionCost(quantity, price);
            return Balance - Commission > transactionCost;
        }

        public void Buy(string symbol, uint quantity, decimal price)
        {
            if (!AccountHoldings.ContainsKey(symbol))
            {
                AccountHoldings.Add(symbol, new Holding(symbol, price, quantity));
            }
            else
            {
                AccountHoldings[symbol].Quantity += quantity;
            }

            decimal transactionCost = GetTransactionCost(quantity, price);
            Balance -= (transactionCost + Commission);
        }

        public bool CanSell(uint quantity, string symbol)
        {
            return AccountHoldings[symbol].Quantity > quantity;
        }

        public void Sell(string symbol, uint quantity)
        {
            AccountHoldings[symbol].Quantity -= quantity;

            decimal transactionCost = GetTransactionCost(quantity, AccountHoldings[symbol].CurrentPrice);
            Balance += (transactionCost - Commission);
        }

        public void UpdateHolding(IStockDataModel stockData)
        {
            AccountHoldings[stockData.Symbol].CurrentPrice = stockData.Open;
        }

        #endregion

        #region Private Methods

        private decimal GetTransactionCost(uint quantity, decimal price)
        {
            return price * quantity;
        }

        #endregion
    }
}
