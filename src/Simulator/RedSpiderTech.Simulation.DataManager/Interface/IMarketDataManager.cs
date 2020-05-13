using System;
using Autofac;
using RedSpiderTech.Simulation.Data.Interface;

namespace RedSpiderTech.Simulation.DataManager.Interface
{
    public interface IMarketDataManager : IStartable, IDisposable
    {
        event EventHandler<IStockDataWithDate> NewStockData;

        void RunMarketData();
    }
}