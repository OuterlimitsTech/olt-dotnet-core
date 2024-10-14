using OLT.Core;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class OltOrderByExtensions
    {

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
            if (sortParams?.PropertyName == null)
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

    }
}
