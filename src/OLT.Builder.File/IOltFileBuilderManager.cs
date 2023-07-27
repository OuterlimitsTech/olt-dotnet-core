using System;
using System.Collections.Generic;

namespace OLT.Core
{
    [Obsolete("Will be removed in 8.x")]
    public interface IOltFileBuilderManager : IOltInjectableSingleton
    {
        List<IOltFileBuilder> GetBuilders();
        IOltFileBase64 Generate<TRequest>(TRequest request, string name) where TRequest : IOltRequest;
    }
}