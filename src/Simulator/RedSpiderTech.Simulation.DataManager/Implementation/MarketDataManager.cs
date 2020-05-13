using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RedSpiderTech.SecuritiesResearch.Common.Interface.Model;
using RedSpiderTech.SecuritiesResearch.DataAccess.Interface;
using RedSpiderTech.Simulation.Common.Interface;
using RedSpiderTech.Simulation.Data.Implementation;
using RedSpiderTech.Simulation.Data.Interface;
using RedSpiderTech.Simulation.DataManager.Interface;

namespace RedSpiderTech.Simulation.DataManager.Implementation
{
    public class MarketDataManager : IMarketDataManager
    {
        #region Private Data

        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ISecurityDataReader _securityDataReader;
        private readonly IEnumerable<string> _symbolCollection;
        private readonly INotificationManager _notificationManager;

        private bool _simulationStartTriggered = false;

        #endregion

        #region Events

        public event EventHandler<IStockDataWithDate> NewStockData;

        #endregion

        #region Public Methods

        public MarketDataManager(ISecurityDataReader securityDataReader, 
                                 INotificationManager notificationManager,
                                 ISimulationParameters simulationParameters)
        {
            _startDate = simulationParameters.SimulationStartDate;
            _endDate = simulationParameters.SimulationEndDate;
            _symbolCollection = simulationParameters.StockPortfolioSymbols;

            _securityDataReader = securityDataReader;
            _notificationManager = notificationManager;

            _notificationManager.SimulationStarted += NotificationManager_SimulationStarted;
        }

        private void NotificationManager_SimulationStarted(object sender, EventArgs e)
        {
            _simulationStartTriggered = true;
        }

        public void RunMarketData()
        {
            while(!_simulationStartTriggered)
            {
                Thread.Sleep(1000);
            }

            IEnumerable<IStockDataModel> allStockData = _securityDataReader.GetSecurityData(_symbolCollection, _startDate, _endDate);
            foreach (DateTime date in GetDateRange(_startDate, _endDate))
            {
                IEnumerable<IStockDataModel> data = allStockData.Where(x => x.TimeStamp.Date == date.Date);

                if(data.Any())
                {
                    NewStockData?.Invoke(this, new StockDataWithDate(data, date));
                }
            }

            _notificationManager.TriggerSimulationEnded();
        }

        public void Start()
        {
            new Task(RunMarketData).Start();
        }

        public void Dispose()
        {
            _notificationManager.SimulationStarted -= NotificationManager_SimulationStarted;
        }

        #endregion

        #region Private Methods

        private IEnumerable<DateTime> GetDateRange(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        #endregion
    }
}
