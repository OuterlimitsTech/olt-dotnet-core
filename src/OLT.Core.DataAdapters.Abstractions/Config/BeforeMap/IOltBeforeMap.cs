using System.Linq;

namespace OLT.Core
{
    public interface IOltBeforeMap<TSource, TDestination> : IOltAdapterMapConfig<TSource, TDestination>
    {
        IQueryable<TSource> BeforeMap(IQueryable<TSource> queryable);
    }
}