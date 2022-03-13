using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLT.Core
{
    public static class OltAutomapperExtensions 
    {
        public static IMappingExpression<TSource, TDestination> BeforeMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, IOltBeforeMap<TSource, TDestination> beforeMap)
            where TSource : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (beforeMap == null)
            {
                throw new ArgumentNullException(nameof(beforeMap));
            }

            OltAdapterMapConfigs.BeforeMap.Register(beforeMap);
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> BeforeMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Func<IQueryable<TSource>, IQueryable<TSource>> func)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(func);
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> AfterMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, IOltAfterMap<TSource, TDestination> afterMap)
            where TSource : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (afterMap == null)
            {
                throw new ArgumentNullException(nameof(afterMap));
            }

            OltAdapterMapConfigs.AfterMap.Register(afterMap);
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> AfterMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Func<IQueryable<TDestination>, IQueryable<TDestination>> func)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            OltAdapterMapConfigs.AfterMap.Register<TSource, TDestination>(func);
            return expression;
        }
    }
}
