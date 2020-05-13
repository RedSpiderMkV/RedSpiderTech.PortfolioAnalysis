using System;
using System.Collections.Generic;
using System.Linq;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Diagnostic;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.DataManager.Interface;
using RedSpiderTech.Simulation.Simulations.Common;
using RedSpiderTech.Simulation.Simulations.Rebalance.Models;
using RedSpiderTech.Simulation.Simulations.Rebalance.Utilities;
using Serilog;

namespace RedSpiderTech.Simulation.Simulations.Rebalance
{
    public class RebalanceSimulationManager : IRebalanceSimulationManager
    {
        #region Events

        public event EventHandler<AccountUpdatedEventArgs> AccountUpdatedEvent;

        #endregion

        #region Private Data

        private readonly IAccount _account;
        private readonly IMarketDataManager _marketDataManager;
        private readonly INotificationManager _notificationManager;
        private readonly ILogger _logger;
        private readonly ICalendarRebalanceScheduleManager _calendarRebalanceScheduleManager;
        private readonly IDiagnosticManager _diagnosticManager;

        #endregion

        #region Public Methods

        public RebalanceSimulationManager(ILogger logger, 
                                          IAppConfigurationManager configurationManager, 
                                          IDiagnosticManager diagnosticManager,
                                          IMarketDataManager marketDataManager, 
                                          INotificationManager notificationManager,
                                          ICalendarRebalanceScheduleManager rebalanceScheduleManager)
        {
            _calendarRebalanceScheduleManager = rebalanceScheduleManager;
            _diagnosticManager = diagnosticManager;
            _marketDataManager = marketDataManager;
            _account = configurationManager.GetAccount();
            _notificationManager = notificationManager;
            _logger = logger;

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

            if (_calendarRebalanceScheduleManager.ShouldRebalance(stockData.Date.Month))
            {
                // Do rebalance
                _diagnosticManager.LogRunTime(DoRebalance);
                //DoRebalance();

                foreach (KeyValuePair<string, IHolding> holding in _account.AccountHoldings)
                {
                    _logger.Information("Contribution from: " + holding.Key + ": " + holding.Value.CurrentPrice * holding.Value.Quantity / 100.0m);
                }

                _logger.Information($"Account valuation date: {stockData.Date.ToString("yyyy-MM-dd")}\tValuation: {_account.CurrentValuation / 100.0m}");
            }
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

        private void DoRebalance()
        {
            decimal tradeableBalance = _account.CurrentValuation - _account.AccountHoldings.Count * _account.Commission;
            decimal targetCapital = tradeableBalance / _account.AccountHoldings.Count;

            IEnumerable<IRebalancingTradeData> underweightHoldings = _account.AccountHoldings.Select(holding =>
            {
                return GetRebalancingTradeData(holding.Value, BuyPredicate, targetCapital);
            }).Where(x => x != null);

            IEnumerable<IRebalancingTradeData> overweightHoldings = _account.AccountHoldings.Select(holding =>
            {
                return GetRebalancingTradeData(holding.Value, SellPredicate, targetCapital);
            }).Where(x => x != null);

            overweightHoldings.ToList().ForEach(SellIfPossible);
            underweightHoldings.ToList().ForEach(BuyIfPossible);
        }

        private static bool SellPredicate(uint quantity, decimal differenceFromTarget)
        {
            return quantity > 0 && differenceFromTarget > 0;
        }

        private static bool BuyPredicate(uint quantity, decimal differenceFromTarget)
        {
            return quantity > 0 && differenceFromTarget < 0;
        }

        private static IRebalancingTradeData GetRebalancingTradeData(IHolding holding, Func<uint, decimal, bool> validation, decimal targetCapital)
        {
            decimal contribution = holding.CurrentPrice * holding.Quantity;
            decimal differenceFromTarget = contribution - targetCapital;
            uint quantity = (uint)(Math.Abs(differenceFromTarget) / holding.CurrentPrice);

            return validation(quantity, differenceFromTarget) ? new RebalancingTradeData(quantity, holding.Symbol, holding.CurrentPrice) : null;
        }

        private void SellIfPossible(IRebalancingTradeData rebalancingTradeData)
        {
            if (_account.CanSell(rebalancingTradeData.Quantity, rebalancingTradeData.Symbol))
            {
                _account.Sell(rebalancingTradeData.Symbol, rebalancingTradeData.Quantity);
            }
        }

        private void BuyIfPossible(IRebalancingTradeData rebalancingTradeData)
        {
            if (_account.CanBuy(rebalancingTradeData.Quantity, rebalancingTradeData.Price))
            {
                _account.Buy(rebalancingTradeData.Symbol, rebalancingTradeData.Quantity, rebalancingTradeData.Price);
            }
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
