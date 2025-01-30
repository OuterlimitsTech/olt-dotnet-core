namespace OLT.Core
{
    /// <summary>
    /// General Http Errors (used for frontend standardization)
    /// </summary>
    public interface IOltErrorHttp
    {
        string? Message { get; }
        List<string> Errors { get; }
    }

}