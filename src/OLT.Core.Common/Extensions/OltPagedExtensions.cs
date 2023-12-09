using OLT.Core;

namespace System.Linq
{
    public static class OltPagedExtensions
    {
        /// <summary>
        /// Returns <see cref="IOltPaged"/> of <see cref="IQueryable"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IOltPaged<T> ToPaged<T>(this IQueryable<T> queryable, IOltPagingParams pagingParams)
        {
            return ToPaged(queryable, pagingParams, null);
        }


        /// <summary>
        /// Returns <see cref="IOltPaged"/> of <see cref="IQueryable"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pagingParams"></param>
        /// <param name="orderBy">Optional</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IOltPaged<T> ToPaged<T>(this IQueryable<T> queryable, IOltPagingParams pagingParams, Func<IQueryable<T>, IQueryable<T>>? orderBy)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(queryable);
            ArgumentNullException.ThrowIfNull(pagingParams);
#else
            OltArgumentNullException.ThrowIfNull(queryable, nameof(queryable));
            OltArgumentNullException.ThrowIfNull(pagingParams, nameof(pagingParams));            
#endif

            var cnt = queryable.Count();

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            var pagedQueryable = queryable
                .Skip((pagingParams.Page - 1) * pagingParams.Size)
                .Take(pagingParams.Size);


            return new OltPagedJson<T>
            {
                Count = cnt,
                Page = pagingParams.Page,
                Size = pagingParams.Size,
                Data = pagedQueryable.ToList()
            };

        }

    }
}
