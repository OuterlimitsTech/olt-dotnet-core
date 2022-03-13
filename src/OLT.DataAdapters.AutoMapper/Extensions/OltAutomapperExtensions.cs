using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLT.Core
{
    public static class OltAutomapperExtensions 
    {
        public static IMappingExpression<TSource, TDestination> BeforeMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, IOltBeforeMap<TSource, TDestination> beforeMap)
            where TSource : class
        {
            OltAdapterMapConfigs.BeforeMap.Register(beforeMap);
            return map;
        }

        public static IMappingExpression<TSource, TDestination> BeforeMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy)
        {
            OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(orderBy);
            return map;
        }

        public static IMappingExpression<TSource, TDestination> AfterMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, IOltAfterMap<TSource, TDestination> afterMap)
            where TSource : class
        {
            OltAdapterMapConfigs.AfterMap.Register(afterMap);
            return map;
        }

        public static IMappingExpression<TSource, TDestination> AfterMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Func<IQueryable<TDestination>, IQueryable<TDestination>> orderBy)
        {
            OltAdapterMapConfigs.AfterMap.Register<TSource, TDestination>(orderBy);
            return map;
        }
    }
}
