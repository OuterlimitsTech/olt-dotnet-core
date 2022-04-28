using System;
using System.Collections.Generic;

namespace OLT.Core
{
    [Obsolete("Move to OltRuleBuilder")]
    public interface IOltRuleManager : IOltInjectableScoped
    {
        TRule GetRule<TRule>() where TRule : class, IOltRule;
        List<TRule> GetRules<TRule>() where TRule : IOltRule;
    }
}