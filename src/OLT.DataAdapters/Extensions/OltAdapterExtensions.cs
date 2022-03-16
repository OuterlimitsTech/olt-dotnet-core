using System;
using System.Linq;

namespace OLT.Core
{

    public static class OltAdapterExtensions
    {
        public static string BuildAdapterName<TObj1, TObj2>()
        {
            return $"{typeof(TObj1).FullName}->{typeof(TObj2).FullName}";
        }

        public static string BuildAfterMapName<TObj1, TObj2>()
        {
            return $"{BuildAdapterName<TObj1, TObj2>()}_AfterMap";
        }

        public static string BuildBeforeMapName<TObj1, TObj2>()
        {
            return $"{BuildAdapterName<TObj1, TObj2>()}_BeforeMap";
        }

        /// <summary>
        /// Sets default OrderBy of <typeparamref name="TSource"/> for <seealso cref="IOltPaged"/> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool WithOrderBy<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func)
            where TSource : class
        {
            return BeforeMap(adapter, func);
           
        }

        /// <summary>
        /// Sets default OrderBy of <typeparamref name="TDestination"/> for Paged Resultsets
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool WithOrderBy<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func)
             where TSource : class
        {
            return AfterMap(adapter, func);            
        }

        public static bool BeforeMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, IOltBeforeMap<TSource, TDestination> beforeMap)            
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (beforeMap == null)
            {
                throw new ArgumentNullException(nameof(beforeMap));
            }

            return OltAdapterMapConfigs.BeforeMap.Register(beforeMap, false);
        }

        public static bool BeforeMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(func, false);            
        }

        public static bool AfterMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, IOltAfterMap<TSource, TDestination> afterMap)
            where TSource : class
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (afterMap == null)
            {
                throw new ArgumentNullException(nameof(afterMap));
            }

            return OltAdapterMapConfigs.AfterMap.Register(afterMap, false);
        }

        public static bool AfterMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return OltAdapterMapConfigs.AfterMap.Register<TSource, TDestination>(func, false);
        }
    }
}
