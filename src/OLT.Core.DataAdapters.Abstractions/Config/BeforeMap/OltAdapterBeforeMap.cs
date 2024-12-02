namespace OLT.Core;

public abstract class OltAdapterBeforeMap<TSource, TDestination> : IOltBeforeMap<TSource, TDestination>
{        
    public abstract IQueryable<TSource> BeforeMap(IQueryable<TSource> queryable);
}