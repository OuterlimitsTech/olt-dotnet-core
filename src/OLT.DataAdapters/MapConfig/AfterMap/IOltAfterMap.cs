using System.Linq;

namespace OLT.Core
{
    public interface IOltAfterMap<TSource, TDestination> : IOltAdapterMapConfig<TSource, TDestination>
    {
        IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable);
    }

}