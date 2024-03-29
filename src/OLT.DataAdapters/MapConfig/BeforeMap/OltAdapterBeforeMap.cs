﻿using System.Linq;

namespace OLT.Core
{
    public abstract class OltAdapterBeforeMap<TSource, TDestination> : OltDisposable, IOltBeforeMap<TSource, TDestination>
    {        
        public abstract IQueryable<TSource> BeforeMap(IQueryable<TSource> queryable);
    }

}