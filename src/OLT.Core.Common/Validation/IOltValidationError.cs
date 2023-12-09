namespace OLT.Core
{

    /// <summary>
    /// Defines general validtion error
    /// </summary>
    public interface IOltValidationError
    {
        string? Message { get; set; }
    }
}