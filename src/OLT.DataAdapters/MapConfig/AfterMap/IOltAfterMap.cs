using System.Linq;

namespace OLT.Core
{
    public interface IOltAfterMap<TSource, TDestination> : IOltAdapterMapConfig
    {
        IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable);
    }

}