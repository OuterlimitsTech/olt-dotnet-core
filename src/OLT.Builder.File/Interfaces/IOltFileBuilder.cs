using System;

namespace OLT.Core
{
    [Obsolete("Will be removed in 8.x")]
    public interface IOltFileBuilder : IOltBuilder
    {
        IOltFileBase64 Build<TRequest>(TRequest request) where TRequest : IOltRequest;
    }
}