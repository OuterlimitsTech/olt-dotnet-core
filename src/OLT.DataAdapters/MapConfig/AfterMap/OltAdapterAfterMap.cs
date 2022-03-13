using System.Linq;

namespace OLT.Core
{
    public abstract class OltAdapterAfterMap<TSource, TDestination> : OltDisposable, IOltAfterMap<TSource, TDestination>
    {
        public string Name => OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
        public abstract IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable);
    }

}