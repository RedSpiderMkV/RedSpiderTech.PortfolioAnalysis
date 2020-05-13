namespace RedSpiderTech.Simulation.Data.Interface
{
    public interface IHolding
    {
        decimal CurrentPrice { get; set; }
        decimal InitialPrice { get; }
        uint Quantity { get; set; }
        string Symbol { get; }
    }
}