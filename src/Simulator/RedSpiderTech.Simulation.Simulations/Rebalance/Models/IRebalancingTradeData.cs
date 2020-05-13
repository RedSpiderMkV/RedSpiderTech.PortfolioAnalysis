namespace RedSpiderTech.Simulation.Simulations.Rebalance.Models
{
    public interface IRebalancingTradeData
    {
        decimal Price { get; }
        uint Quantity { get; }
        string Symbol { get; }
    }
}