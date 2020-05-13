namespace RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface
{
    public interface IChartDataPoint<T>
    {
        T XData { get; }
        decimal YData { get; }
    }
}