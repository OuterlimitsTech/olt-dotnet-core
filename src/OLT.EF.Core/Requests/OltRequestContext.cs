using System;

namespace OLT.Core
{
    [Obsolete("Being Removed in 9.x")]
    public abstract class OltRequestContext<TContext> : IOltRequest
        where TContext : class, IOltDbContext
    {
        protected OltRequestContext(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; }
    }

    [Obsolete("Being Removed in 9.x")]
    public abstract class OltRequestContext<TContext, TValue> : OltRequest<TValue>
        where TContext : class, IOltDbContext
    {
        protected OltRequestContext(TContext context, TValue value) : base(value)
        {
            Context = context;
        }

        public TContext Context { get; }


    }
}