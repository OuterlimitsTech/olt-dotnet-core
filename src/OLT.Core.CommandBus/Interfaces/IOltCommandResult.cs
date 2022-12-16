namespace OLT.Core
{
    public interface IOltCommandBusResult
    {
        string CorrelationId { get; }
        string ActionName { get; }
        IOltCommandResult CommandResult { get; }
    }

    public interface IOltCommandResult
    {
        TResult GetResult<TResult>();
    }
}