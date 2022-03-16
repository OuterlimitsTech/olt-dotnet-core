using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public static class OltAdapterMapConfigs 
    {       

        //public static bool ExceptionsEnabled = false;

        public static class BeforeMap
        {
            private static readonly ConcurrentDictionary<string, IOltAdapterMapConfig> _mapConfigs = new ConcurrentDictionary<string, IOltAdapterMapConfig>();
                        
            public static bool IsRegistered<TSource, TDestination>()
            {
                var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
                return _mapConfigs.ContainsKey(name);
            }

            public static IQueryable<TSource> Apply<TSource, TDestination>(IQueryable<TSource> queryable)
            {
                if (IsRegistered<TSource, TDestination>())
                {
                    var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
                    queryable = (_mapConfigs[name] as IOltBeforeMap<TSource, TDestination>).BeforeMap(queryable);
                }
                return queryable;
            }

            public static bool Register<TSource, TDestination>(IOltBeforeMap<TSource, TDestination> configMap, bool throwException)
            {
                var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
                if (!_mapConfigs.ContainsKey(name))
                {
                    return _mapConfigs.TryAdd(name, configMap);                    
                }

                if (throwException)
                {
                    throw new OltAdapterMapConfigExists<TSource, TDestination>(configMap);
                }

                return false;   
            }

            public static bool Register<TSource, TDestination>(Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func, bool throwException)
            {
                return Register(new OltBeforeMapOrderBy<TSource, TDestination>(func), throwException);
            }           


        }

        public static class AfterMap
        {
            private static readonly ConcurrentDictionary<string, IOltAdapterMapConfig> _mapConfigs = new ConcurrentDictionary<string, IOltAdapterMapConfig>();

            public static bool IsRegistered<TSource, TDestination>()
            {
                var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
                return _mapConfigs.ContainsKey(name);
            }

            public static IQueryable<TDestination> Apply<TSource, TDestination>(IQueryable<TDestination> queryable)
            {                
                if (IsRegistered<TSource, TDestination>())
                {
                    var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
                    queryable = (_mapConfigs[name] as IOltAfterMap<TSource, TDestination>).AfterMap(queryable);
                }                
                return queryable;
            }

            public static bool Register<TSource, TDestination>(IOltAfterMap<TSource, TDestination> configMap, bool throwException)
            {
                var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
                if (!_mapConfigs.ContainsKey(name))
                {
                    return _mapConfigs.TryAdd(name, configMap);
                }

                if (throwException)
                {
                    throw new OltAdapterMapConfigExists<TSource, TDestination>(configMap);
                }

                return false;                
            }

            public static bool Register<TSource, TDestination>(Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func, bool throwException)
            {
                return Register(new OltAfterMapOrderBy<TSource, TDestination>(func), throwException);
            }

        }


    
    }

   


}