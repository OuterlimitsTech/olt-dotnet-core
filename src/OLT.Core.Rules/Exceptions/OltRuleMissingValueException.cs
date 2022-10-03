using System;

namespace OLT.Core
{
    public class OltRuleMissingValueException<TValue> : OltRuleCanRunException
      where TValue : IConvertible
    {
        public OltRuleMissingValueException(IOltRule rule, string key) : base($"{rule.RuleName} requires {key} value of type {typeof(TValue).FullName}")
        {
        }
    }
}