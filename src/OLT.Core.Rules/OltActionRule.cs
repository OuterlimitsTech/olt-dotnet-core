using System;

namespace OLT.Core
{
    [Obsolete("Move to OltActionRule")]
    public abstract class OltRuleAction : OltRule, IOltRuleAction
    {
        public abstract IOltRuleResult Execute(IOltRequest request);
    }

    [Obsolete("Move to OltActionRule")]
    public abstract class OltRuleAction<TRequest> : OltRule, IOltRuleAction<TRequest>
        where TRequest : class, IOltRequest
    {
        public abstract IOltRuleResult Execute(TRequest request);
    }

}
