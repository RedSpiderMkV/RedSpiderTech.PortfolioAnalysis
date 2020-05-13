namespace RedSpiderTech.Simulation.Simulations.Rebalance.Models
{
    public class RebalancingTradeData : IRebalancingTradeData
    {
        #region Properties

        public uint Quantity { get; }
        public string Symbol { get; }
        public decimal Price { get; }

        #endregion

        #region Public Methods

        public RebalancingTradeData(uint quantity, string symbol, decimal price)
        {
            Quantity = quantity;
            Symbol = symbol;
            Price = price;
        }

        #endregion
    }
}
