
namespace RedSpiderTech.SecuritiesResearch.DataAccess.Interface
{
    public interface IMySqlCommandWrapperFactory
    {
        IMySqlCommandWrapper GetCommandWrapper(string commandString, IMySqlConnectionWrapper connectionWrapper);
    }
}