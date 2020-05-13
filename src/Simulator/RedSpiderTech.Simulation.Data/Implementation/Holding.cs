using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.Data.Implementation
{
    public class Holding : IHolding
    {
        public uint Quantity { get; set; }
        public decimal InitialPrice { get; }
        public string Symbol { get; }

        public decimal CurrentPrice { get; set; }

        public Holding(string symbol, decimal price, uint quantity)
        {
            Symbol = symbol;
            InitialPrice = price;
            CurrentPrice = price;
            Quantity = quantity;
        }
    }
}
