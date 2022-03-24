namespace OLT.Core
{
    public interface IOltRuleAction : IOltRule
    {
        IOltRuleResult Execute(IOltRequest request);
    }

    public interface IOltRuleAction<in TRequest> : IOltRule
        where TRequest : IOltRequest
    {
        IOltRuleResult Execute(TRequest request);
    }

    public interface IOltRuleAction<in TRequest, out TResult> : IOltRule
        where TRequest : IOltRequest
        where TResult : IOltRuleResult
    {
        TResult Execute(TRequest request);
    }
}