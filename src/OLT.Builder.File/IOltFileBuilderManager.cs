using System;
using System.Collections.Generic;

namespace OLT.Core
{
    [Obsolete("Removing in 8.x")]
    public interface IOltFileBuilderManager : IOltInjectableSingleton
    {
        List<IOltFileBuilder> GetBuilders();
        IOltFileBase64 Generate<TRequest>(TRequest request, string name) where TRequest : IOltRequest;
    }
}