using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.DataManager.Interface;
using RedSpiderTech.Simulation.Simulations.Common;
using Serilog;

namespace RedSpiderTech.Simulation.Simulations.BuyAndHold
{
    public class BuyAndHoldSimulationManager : IBuyAndHoldSimulationManager
    {
        #region Events

        public event EventHandler<AccountUpdatedEventArgs> AccountUpdatedEvent;

        #endregion

        #region Private Data

        private readonly IAccount _account;
        private readonly IMarketDataManager _marketDataManager;
        private readonly INotificationManager _notificationManager;
        private readonly ILogger _logger;
        private readonly IDiagnosticManager _diagnosticManager;

        #endregion

        #region Public Methods

        public BuyAndHoldSimulationManager(ILogger logger, IAppConfigurationManager configurationManager, IMarketDataManager marketDataManager, INotificationManager notificationManager, IDiagnosticManager diagnosticManager)
        {
            _logger = logger;
            _account = configurationManager.GetAccount();
            _marketDataManager = marketDataManager;
            _notificationManager = notificationManager;
            _diagnosticManager = diagnosticManager;

            _notificationManager.SimulationEnded += NotificationManager_SimulationEnded;
        }

        public void Dispose()
        {
            _marketDataManager.NewStockData -= MarketDataManager_NewStockData;
        }

        public void Start()
        {
            _marketDataManager.NewStockData += MarketDataManager_NewStockData;
        }

        #endregion

        #region Private Methods

        private void MarketDataManager_NewStockData(object sender, IStockDataWithDate stockData)
        {
            if (!stockData.StockDataCollection.Any())
            {
                return;
            }

            UpdateAccountHoldings(stockData);

            foreach (KeyValuePair<string, IHolding> holding in _account.AccountHoldings)
            {
                _logger.Information("Contribution from: " + holding.Key + ": " + holding.Value.CurrentPrice * holding.Value.Quantity / 100.0m);
            }

            _logger.Information($"Account valuation date: {stockData.Date.ToString("yyyy-MM-dd")}\tValuation: {_account.CurrentValuation / 100.0m}");
        }

        private void UpdateAccountHoldings(IStockDataWithDate stockData)
        {
            int numberOfStocks = stockData.StockDataCollection.Count();
            decimal amountAvailablePerStock = _account.GetAvailableBalanceForBuying((uint)numberOfStocks) / numberOfStocks;
            foreach (IStockDataModel data in stockData.StockDataCollection)
            {
                if (!_account.AccountHoldings.ContainsKey(data.Symbol))
                {
                    // determine quantity - equal weighting
                    uint quantity = (uint)(amountAvailablePerStock / (data.Open));
                    if (_account.CanBuy(quantity, data.Open))
                    {
                        _account.Buy(data.Symbol, quantity, data.Open);
                    }
                }

                _account.UpdateHolding(data);
            }

            AccountUpdatedEvent?.Invoke(this, new AccountUpdatedEventArgs(_account, stockData.Date));
        }

        private void NotificationManager_SimulationEnded(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, IHolding> holding in _account.AccountHoldings)
            {
                _logger.Information("Final Contribution from: " + holding.Key + ": " + holding.Value.CurrentPrice * holding.Value.Quantity / 100.0m);
            }

            _logger.Information($"Account Final Valuation: {_account.CurrentValuation / 100.0m}");
        }

        #endregion
    }
}
