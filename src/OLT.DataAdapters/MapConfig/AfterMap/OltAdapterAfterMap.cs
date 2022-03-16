using System.Linq;

namespace OLT.Core
{
    public abstract class OltAdapterAfterMap<TSource, TDestination> : OltDisposable, IOltAfterMap<TSource, TDestination>
    {        
        public abstract IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable);
    }

}