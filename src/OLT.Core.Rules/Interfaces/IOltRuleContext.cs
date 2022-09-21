namespace OLT.Core
{
    public interface IOltRuleContext : IOltInjectableScoped
    {
        IOltRuleServiceManager ServiceManager { get; }
    }
}