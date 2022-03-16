using System;
using System.Linq;

namespace OLT.Core
{
    public interface IOltAdapterPaged<TEntity, TDestination> : IOltAdapterQueryable<TEntity, TDestination>
    {
        IQueryable<TEntity> DefaultOrderBy(IQueryable<TEntity> queryable);
    }
}