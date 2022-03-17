using System;
using System.Linq;

namespace OLT.Core
{
    public abstract class OltAdapterPaged<TSource, TDestination> : OltAdapterQueryable<TSource, TDestination>, IOltAdapterPaged<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        protected OltAdapterPaged()
        {
            this.WithOrderBy(DefaultOrderBy);
        }

        public abstract IOrderedQueryable<TSource> DefaultOrderBy(IQueryable<TSource> queryable);
    }
}