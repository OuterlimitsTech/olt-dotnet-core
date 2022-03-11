using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public interface IOltAdapterResolver : IOltInjectableSingleton
    {
        IOltAdapter<TSource, TDestination> GetAdapter<TSource, TDestination>(bool throwException = true);

        
        bool CanProjectTo<TEntity, TDestination>();
        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source);

        bool CanMap<TSource, TDestination>();
        List<TDestination> Map<TSource, TDestination>(List<TSource> source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    }
}