using OLT.Core;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class OltQueryableExtensions
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

        /// <summary>
        /// Order <see cref="IQueryable"/> by name of property (<paramref name="memberPath"/>) in class 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="memberPath"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByPropertyName<T>(this IQueryable<T> queryable, string memberPath, bool isAscending)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);
            var methodCall = Expression.Call(typeof(Queryable), isAscending ? "OrderBy" : "OrderByDescending", new[] { parameter.Type, member.Type }, queryable.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)queryable.Provider.CreateQuery(methodCall);
        }


        /// <summary>
        /// Order <see cref="IQueryable"/> by <see cref="IOltSortParams.PropertyName"/> in class 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="sortParams"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, IOltSortParams sortParams)
        {
            if (sortParams == null)
            {
                throw new ArgumentNullException(nameof(sortParams));
            }
            return OrderByPropertyName(queryable, sortParams.PropertyName, sortParams.IsAscending);
        }


        /// <summary>
        /// Order <see cref="IQueryable"/> by <see cref="IOltSortParams.PropertyName"/> in class
        /// Defaults to <paramref name="defaultOrderBy"/> is <paramref name="sortParams"/> is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="sortParams"></param>
        /// <param name="defaultOrderBy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, IOltSortParams sortParams, Func<IQueryable<T>, IQueryable<T>> defaultOrderBy)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (defaultOrderBy == null)
            {
                throw new ArgumentNullException(nameof(defaultOrderBy));
            }

            if (sortParams?.PropertyName != null && !string.IsNullOrEmpty(sortParams.PropertyName))
            {
                return OrderBy(queryable, sortParams);
            }
            return defaultOrderBy(queryable);
        }


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
        public static IOltPaged<T> ToPaged<T>(this IQueryable<T> queryable, IOltPagingParams pagingParams, Func<IQueryable<T>, IQueryable<T>> orderBy)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (pagingParams == null)
            {
                throw new ArgumentNullException(nameof(pagingParams));
            }

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
