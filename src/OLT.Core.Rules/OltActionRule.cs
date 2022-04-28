using System;

namespace OLT.Core
{
    [Obsolete("Move to OltRuleBuilder")]
    public abstract class OltRuleAction : OltRule, IOltRuleAction
    {
        public abstract IOltRuleResult Execute(IOltRequest request);
    }

    [Obsolete("Move to OltRuleBuilder")]
    public abstract class OltRuleAction<TRequest> : OltRule, IOltRuleAction<TRequest>
        where TRequest : class, IOltRequest
    {
        public abstract IOltRuleResult Execute(TRequest request);
    }

    [Obsolete("Move to OltRuleBuilder")]
    public abstract class OltRuleAction<TRequest, TResult> : OltRule, IOltRuleAction<TRequest, TResult>
        where TRequest : IOltRequest
        where TResult : IOltRuleResult
    {
        public abstract TResult Execute(TRequest request);
    }

}
