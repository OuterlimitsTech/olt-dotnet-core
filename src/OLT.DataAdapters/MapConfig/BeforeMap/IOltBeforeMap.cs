using System.Linq;

namespace OLT.Core
{
    public interface IOltBeforeMap<TSource, TDestination> : IOltAdapterMapConfig
    {
        IQueryable<TSource> BeforeMap(IQueryable<TSource> queryable);
    }
}