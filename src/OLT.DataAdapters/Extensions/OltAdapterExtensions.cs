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

        public static IOltAdapter<TSource, TDestination> BeforeMap<TSource, TDestination>(this IOltAdapter<TSource, TDestination> adapter, IOltBeforeMap<TSource, TDestination> beforeMap)            
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (beforeMap == null)
            {
                throw new ArgumentNullException(nameof(beforeMap));
            }

            OltAdapterMapConfigs.BeforeMap.Register(beforeMap, false);
            return adapter;
        }

        public static IOltAdapter<TSource, TDestination> BeforeMap<TSource, TDestination>(this IOltAdapter<TSource, TDestination> adapter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(func, false);
            return adapter;
        }

        public static IOltAdapterQueryable<TSource, TDestination> AfterMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, IOltAfterMap<TSource, TDestination> afterMap)
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

            OltAdapterMapConfigs.AfterMap.Register(afterMap, false);
            return adapter;
        }

        public static IOltAdapterQueryable<TSource, TDestination> AfterMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            OltAdapterMapConfigs.AfterMap.Register<TSource, TDestination>(func, false);
            return adapter;
        }
    }
}
