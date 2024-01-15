namespace OLT.Core
{
    public interface IOltCommandResult
    {
        TResult GetResult<TResult>();
    }
}