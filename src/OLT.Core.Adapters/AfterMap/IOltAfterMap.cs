using System.Linq;

namespace OLT.Core
{
    public interface IOltAfterMap
    {
        string Name { get; }
    }

    public interface IOltAfterMap<TSource, TDestination> : IOltAfterMap
    {
        IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable);
    }
}