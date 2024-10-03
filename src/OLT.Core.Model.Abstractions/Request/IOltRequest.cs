using System;

namespace OLT.Core
{

    /// <summary>
    /// Marker class for a Request
    /// </summary>
    [Obsolete("Being Removed in 9.x")]
    public interface IOltRequest
    {

    }

    /// <summary>
    /// Request with a passed value
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Obsolete("Being Removed in 9.x")]
    public interface IOltRequest<out TValue> : IOltRequest
    {
        TValue Value { get; }
    }
    
}