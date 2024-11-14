using OLT.Core;

namespace System.Linq
{
    public static class OltWhereExtensions
    {

        /// <summary>
        /// Builds <see cref="IQueryable"/> for provieded <paramref name="searchers"/>)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searchers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> queryable, params IOltSearcher<TSource>[] searchers)
            where TSource : class, IOltEntity
        {
            ArgumentNullException.ThrowIfNull(queryable);
            ArgumentNullException.ThrowIfNull(searchers);
  
            foreach(var searcher in searchers)
            {
                queryable = queryable.Where(searcher);
            }

            return queryable;
        }


        /// <summary>
        /// Builds <see cref="IQueryable"/> for provieded <paramref name="searcher"/>)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searcher"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> queryable, IOltSearcher<TSource> searcher)
            where TSource : class, IOltEntity
        {
            ArgumentNullException.ThrowIfNull(queryable);
            ArgumentNullException.ThrowIfNull(searcher);

            return searcher.BuildQueryable(queryable);
        }


    }
}
