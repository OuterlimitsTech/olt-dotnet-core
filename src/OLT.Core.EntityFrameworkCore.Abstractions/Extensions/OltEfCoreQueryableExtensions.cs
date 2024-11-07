using Microsoft.EntityFrameworkCore;

namespace OLT.Core
{
    public static class OltEfCoreQueryableExtensions
    {
        public static Task<IOltPaged<TDestination>> ToPagedAsync<TDestination>(this IQueryable<TDestination> queryable, IOltPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (pagingParams == null)
            {
                throw new ArgumentNullException(nameof(pagingParams));
            }

            return ToPagedInternalAsync(queryable, pagingParams, cancellationToken);
        }

        private static async Task<IOltPaged<TDestination>> ToPagedInternalAsync<TDestination>(this IQueryable<TDestination> queryable, IOltPagingParams pagingParams, CancellationToken cancellationToken = default)
        {

            var cnt = await queryable.CountAsync(cancellationToken);

            var pagedQueryable = queryable
                .Skip((pagingParams.Page - 1) * pagingParams.Size)
                .Take(pagingParams.Size);


            return new OltPagedJson<TDestination>
            {
                Count = cnt,
                Page = pagingParams.Page,
                Size = pagingParams.Size,
                Data = await pagedQueryable.ToListAsync(cancellationToken)
            };
        }
    }
}