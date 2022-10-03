namespace OLT.Core
{
    public interface IOltRuleServiceManager : IOltServiceManager
    {
        TService GetService<TService>() where TService : notnull;
    }
}