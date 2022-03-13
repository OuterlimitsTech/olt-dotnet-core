using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public static class OltAdapterMapConfigs 
    {
        private static readonly List<IOltAdapterMapConfig> _mapConfigs = new List<IOltAdapterMapConfig>();


        public static IQueryable<TSource> ApplyBeforeMaps<TSource, TDestination>(IQueryable<TSource> queryable)
        {
            var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
            _mapConfigs.Where(p => p.Name == name)
                .ToList()
                .ForEach(item =>
                {
                    if (item is IOltBeforeMap<TSource, TDestination> beforeMap)
                    {
                        queryable = beforeMap.BeforeMap(queryable);
                    }
                });

            return queryable;
        }


        public static IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
            _mapConfigs.Where(p => p.Name == name)
                .ToList()
                .ForEach(item =>
                {
                    if (item is IOltAfterMap<TSource, TDestination> afterMap)
                    {
                        queryable = afterMap.AfterMap(queryable);
                    }
                });

            return queryable;
        }

        public static class BeforeMap
        {
            public static void Register<TSource, TDestination>(IOltBeforeMap<TSource, TDestination> afterMap)
            {
                _mapConfigs.Add(afterMap);
            }

            public static void Register<TSource, TDestination>(Func<IQueryable<TSource>, IQueryable<TSource>> func)
            {
                _mapConfigs.Add(new OltBeforeMapOrderBy<TSource, TDestination>(func));
            }

        }

        public static class AfterMap
        {
            public static void Register<TSource, TDestination>(IOltAfterMap<TSource, TDestination> afterMap)
            {
                _mapConfigs.Add(afterMap);
            }

            public static void Register<TSource, TDestination>(Func<IQueryable<TDestination>, IQueryable<TDestination>> func)
            {
                _mapConfigs.Add(new OltAfterMapOrderBy<TSource, TDestination>(func));
            }

        }


    
    }

   


}