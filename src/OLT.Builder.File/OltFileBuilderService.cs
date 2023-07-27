using System;

namespace OLT.Core
{
    [Obsolete("Removing in 8.x")]
    public abstract class OltFileBuilderService : OltDisposable, IOltFileBuilder, IOltCoreService
    {
        public abstract string BuilderName { get; }
        public abstract IOltFileBase64 Build<TRequest>(TRequest request) where TRequest : IOltRequest;
    }
}