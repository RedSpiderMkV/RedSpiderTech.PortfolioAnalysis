namespace RedSpiderTech.SecuritiesResearch.Common.Interface.Model
{
    public interface ISecurityStaticDataModel
    {
        IExchangeStaticDataModel ExchangeData { get; }
        string ShortName { get; }
        string Symbol { get; }
    }
}