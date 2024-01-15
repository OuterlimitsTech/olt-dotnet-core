namespace OLT.Core
{
    public interface IOltCommandBusResult
    {
        string ActionName { get; }
        IOltCommandResult CommandResult { get; }
    }
}