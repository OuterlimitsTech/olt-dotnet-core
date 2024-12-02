namespace OLT.Core
{
    public abstract class OltAdapterQueryable<TSource, TModel> : OltAdapter<TSource, TModel>, IOltAdapterQueryable<TSource, TModel>
        where TSource : class, new()
        where TModel : class, new()
    {
        public abstract IQueryable<TModel> Map(IQueryable<TSource> queryable);
    }
}