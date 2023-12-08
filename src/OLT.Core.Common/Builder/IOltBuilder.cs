using System;

namespace OLT.Core
{
    [Obsolete("Will be removed in 8.x")]
    public interface IOltBuilder : IDisposable
    {
        string BuilderName { get; }
    }
}