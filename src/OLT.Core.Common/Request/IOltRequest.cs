namespace OLT.Core
{

    /// <summary>
    /// Marker class for a Request
    /// </summary>
    public interface IOltRequest
    {

    }

    /// <summary>
    /// Request with a passed value
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IOltRequest<out TValue> : IOltRequest
    {
        TValue Value { get; }
    }
    
}