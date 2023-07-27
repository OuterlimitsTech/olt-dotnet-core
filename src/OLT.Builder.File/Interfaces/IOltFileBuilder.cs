using System;

namespace OLT.Core
{
    [Obsolete("Removing in 8.x")]
    public interface IOltFileBuilder : IOltBuilder
    {
        IOltFileBase64 Build<TRequest>(TRequest request) where TRequest : IOltRequest;
    }
}