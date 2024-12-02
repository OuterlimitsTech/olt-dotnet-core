namespace OLT.Core
{
    public interface IOltAdapterPaged<TEntity, TDestination> : IOltAdapterQueryable<TEntity, TDestination>
    {
        IOrderedQueryable<TEntity> DefaultOrderBy(IQueryable<TEntity> queryable);
    }
}