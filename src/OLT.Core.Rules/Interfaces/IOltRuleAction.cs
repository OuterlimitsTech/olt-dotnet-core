using System;

namespace OLT.Core
{
    [Obsolete("Move to OltRuleBuilder")]
    public interface IOltRuleAction : IOltRule
    {
        IOltRuleResult Execute(IOltRequest request);
    }

    [Obsolete("Move to OltRuleBuilder")]
    public interface IOltRuleAction<in TRequest> : IOltRule
        where TRequest : IOltRequest
    {
        IOltRuleResult Execute(TRequest request);
    }

    [Obsolete("Move to OltRuleBuilder")]
    public interface IOltRuleAction<in TRequest, out TResult> : IOltRule
        where TRequest : IOltRequest
        where TResult : IOltRuleResult
    {
        TResult Execute(TRequest request);
    }
}