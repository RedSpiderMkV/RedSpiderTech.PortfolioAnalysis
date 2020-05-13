using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface;

namespace RedSpiderTech.SecuritiesResearch.Charts.ChartData.Implementation
{
    public class ChartDataPoint<T> : IChartDataPoint<T>
    {
        #region Properties

        public T XData { get; }
        public decimal YData { get; }

        #endregion

        #region Public Methods

        public ChartDataPoint(T xData, decimal yData)
        {
            XData = xData;
            YData = yData;
        }

        #endregion
    }
}
