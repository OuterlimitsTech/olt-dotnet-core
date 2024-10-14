namespace OLT.Core
{
    /// <summary>
    /// Defines general validation error
    /// </summary>
    public interface IOltValidationError
    {
        string? Message { get; set; }
    }
}