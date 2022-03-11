using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public static class OltAfterMapConfig
    {
        private static readonly List<IOltAfterMap> _afterMapConfigs = new List<IOltAfterMap>();

        public static IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            var name = $"{typeof(TSource).FullName}->{typeof(TDestination).FullName}";
            _afterMapConfigs.Where(p => p.Name == name)
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

        public static void Register<TSource, TDestination>(IOltAfterMap<TSource, TDestination> afterMap)
        {
            _afterMapConfigs.Add(afterMap);
        }

        public static void Register<TSource, TDestination>(Func<IQueryable<TDestination>, IQueryable<TDestination>> orderBy)
        {
            _afterMapConfigs.Add(new OltAfterMapOrderBy<TSource, TDestination>(orderBy));
        }

    }
}