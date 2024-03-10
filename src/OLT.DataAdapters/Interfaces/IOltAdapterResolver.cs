using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public interface IOltAdapterResolver : IOltInjectableSingleton
    {
        IQueryable<TSource> ApplyDefaultOrderBy<TSource, TDestination>(IQueryable<TSource> queryable);

        IOltAdapter<TSource, TDestination>? GetAdapter<TSource, TDestination>(bool throwException = true);


        /// <summary>
        /// Checks for a <see cref="IQueryable" /> <see cref="IOltAdapter"/> or map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        bool CanProjectTo<TSource, TDestination>();

        /// <summary>
        /// Maps using <see cref="IOltAdapter"/> of <see cref="IQueryable"/> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        /// <exception cref="OltAdapterNotFoundException"></exception>
        /// <exception cref="OltException"></exception>
        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, Action<OltAdapterActionConfig>? configAction = null);

        bool CanMap<TSource, TDestination>();
        List<TDestination> Map<TSource, TDestination>(List<TSource> source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    }
}