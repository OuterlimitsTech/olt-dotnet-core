namespace OLT.Core
{
    public abstract class OltRuleAction : OltRule, IOltRuleAction
    {
        public abstract IOltResult Execute(IOltRequest request);
    }

    public abstract class OltRuleAction<TRequest> : OltRule, IOltRuleAction<TRequest>
        where TRequest : class, IOltRequest
    {
        public abstract IOltResult Execute(TRequest request);
    }

    public abstract class OltRuleAction<TRequest, TResult> : OltRule, IOltRuleAction<TRequest, TResult>
        where TRequest : IOltRequest
        where TResult : IOltResult
    {
        public abstract TResult Execute(TRequest request);
    }

}
