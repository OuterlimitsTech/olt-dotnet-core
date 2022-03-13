﻿using System;
using System.Linq;

namespace OLT.Core
{
    [Obsolete("Move ToPaged Extension to BeforeMap or AfterMap")]
    public abstract class OltAdapterPaged<TSource, TDestination> : OltAdapterQueryable<TSource, TDestination>, IOltAdapterPaged<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {

        public abstract IQueryable<TSource> DefaultOrderBy(IQueryable<TSource> queryable);

        public virtual IQueryable<TSource> OrderBy(IQueryable<TSource> queryable, IOltSortParams sortParams = null)
        {
            return OltOrderByExtensions.OrderBy(queryable, sortParams, DefaultOrderBy);
        }

        public virtual IOltPaged<TDestination> Map(IQueryable<TSource> queryable, IOltPagingParams pagingParams, IOltSortParams sortParams = null)
        {
            return this.Map(queryable, pagingParams, sortQueryable => OrderBy(sortQueryable, sortParams));
        }

        public virtual IOltPaged<TDestination> Map(IQueryable<TSource> queryable, IOltPagingParams pagingParams, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy)
        {
            queryable = OltAdapterMapConfigs.ApplyBeforeMaps<TSource, TDestination>(queryable);
            var mapped = OltAdapterMapConfigs.ApplyAfterMaps<TSource, TDestination>(Map(orderBy(queryable)));
            return OltPagedExtensions.ToPaged(mapped, pagingParams);
        }

    }
}