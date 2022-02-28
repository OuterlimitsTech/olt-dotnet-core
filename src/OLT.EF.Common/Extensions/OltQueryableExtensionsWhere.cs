using OLT.Core;

namespace System.Linq
{
    public static class OltQueryableExtensionsWhere
    {
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> queryable, params IOltSearcher<TSource>[] searchers)
            where TSource : class, IOltEntity
        {
            var list = searchers.ToList();
            list.ForEach(searcher => { queryable = queryable.Where(searcher); });
            return queryable;
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> queryable, IOltSearcher<TSource> searcher)
            where TSource : class, IOltEntity
        {
            return searcher.BuildQueryable(queryable);
        }

    }
}
