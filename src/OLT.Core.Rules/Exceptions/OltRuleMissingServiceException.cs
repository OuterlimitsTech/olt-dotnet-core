namespace OLT.Core
{
    public class OltRuleMissingServiceException<TService> : OltRuleCanRunException
        where TService : class, IOltCoreService
    {
        public OltRuleMissingServiceException(IOltRule rule) : base($"{rule.RuleName} requires {typeof(TService).FullName}")
        {
        }
    }
}