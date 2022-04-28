namespace OLT.Core
{
    public class OltRuleMissingParameterException<TParameter> : OltRuleCanRunException
        where TParameter : class
    {
        public OltRuleMissingParameterException(IOltRule rule) : base($"{rule.RuleName} requires {typeof(TParameter).FullName}")
        {
        }
    }
}