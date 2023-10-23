using System;

namespace OLT.Core
{
    [Obsolete("Will be removed in 8.x")]
    public abstract class OltFileBuilder : OltDisposable, IOltFileBuilder, IOltInjectableSingleton
    {
        public abstract string BuilderName { get; }
        public abstract IOltFileBase64 Build<TRequest>(TRequest request) where TRequest: IOltRequest;
    }

    [Obsolete("Will be removed in 8.x")]
    public abstract class OltFileBuilder<TBuilderRequest> : OltFileBuilder
        where TBuilderRequest : class, IOltRequest
    {
        public override IOltFileBase64 Build<TRequest>(TRequest request)
        {
            var obj = request as TBuilderRequest;
            return this.Build(obj);
        }
        protected abstract IOltFileBase64 Build(TBuilderRequest request);
    }
}