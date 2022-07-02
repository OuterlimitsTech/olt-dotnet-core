namespace OLT.Core
{
    public interface IOltGenericFilter
    {
        string Key { get; }
        bool HasValue(IOltGenericParameter parameters);
    }

    public interface IOltGenericFilter<TEntity> : IOltGenericFilter, IOltEntityQueryBuilder<TEntity>  
        where TEntity : class, IOltEntity
    {

    }
}
