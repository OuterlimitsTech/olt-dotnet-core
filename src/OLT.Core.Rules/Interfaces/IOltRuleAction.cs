namespace OLT.Core
{
    public interface IOltRuleAction : IOltRule
    {
        IOltResult Execute(IOltRequest request);
    }

    public interface IOltRuleAction<in TRequest> : IOltRule
        where TRequest : IOltRequest
    {
        IOltResult Execute(TRequest request);
    }

    public interface IOltRuleAction<in TRequest, out TResult> : IOltRule
        where TRequest : IOltRequest
        where TResult : IOltResult
    {
        TResult Execute(TRequest request);
    }
}