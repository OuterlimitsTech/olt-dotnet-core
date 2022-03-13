using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public interface IOltAdapterResolver : IOltInjectableSingleton
    {
        IOltAdapter<TSource, TDestination> GetAdapter<TSource, TDestination>(bool throwException = true);

        IQueryable<TSource> ApplyBeforeMaps<TSource, TDestination>(IQueryable<TSource> queryable);
        IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable);

        bool CanProjectTo<TSource, TDestination>();

        /// <summary>
        /// Applies <see cref="ApplyBeforeMaps"/>, <see cref="IOltAdapter"/> <see cref="IQueryable"/> Map, and <see cref="ApplyAfterMaps"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="OltAdapterNotFoundException"></exception>
        /// <exception cref="OltException"></exception>
        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source);

        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams) where TSource : class;
        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy) where TSource : class;

        [Obsolete("Move to BeforeMap or AfterMap")]
        bool CanMapPaged<TSource, TDestination>();


        bool CanMap<TSource, TDestination>();
        List<TDestination> Map<TSource, TDestination>(List<TSource> source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    }
}