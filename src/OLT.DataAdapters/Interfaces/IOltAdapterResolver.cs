using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public interface IOltAdapterResolver : IOltInjectableSingleton
    {
        IQueryable<TSource> ApplyDefaultOrderBy<TSource, TDestination>(IQueryable<TSource> queryable);

        IOltAdapter<TSource, TDestination> GetAdapter<TSource, TDestination>(bool throwException = true);

        //IQueryable<TSource> ApplyBeforeMaps<TSource, TDestination>(IQueryable<TSource> queryable);
        //IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable);


        /// <summary>
        /// Checks for a <see cref="IQueryable" /> <see cref="IOltAdapter"/> or map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
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

        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, bool applyConfigMaps);

        bool CanMap<TSource, TDestination>();
        List<TDestination> Map<TSource, TDestination>(List<TSource> source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    }
}