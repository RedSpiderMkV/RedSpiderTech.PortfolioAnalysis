namespace RedSpiderTech.Simulation.Simulations.Rebalance.Utilities
{
    public class CalendarRebalanceScheduleManager : ICalendarRebalanceScheduleManager
    {
        #region Private Data

        private int _currentMonth;

        #endregion

        #region Public Methods

        public CalendarRebalanceScheduleManager()
        {
            _currentMonth = -1;
        }

        public bool ShouldRebalance(int newMonth)
        {
            if (newMonth != _currentMonth)
            {
                _currentMonth = newMonth;
                return true;
            }

            return false;
        }

        #endregion
    }
}
