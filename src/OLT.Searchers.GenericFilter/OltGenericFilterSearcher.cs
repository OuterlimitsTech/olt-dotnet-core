using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public abstract class OltGenericFilterSearcher<TEntity> : OltSearcher<TEntity>
       where TEntity : class, IOltEntity
    {
        protected OltGenericFilterSearcher(IOltGenericParameter parameters, List<IOltGenericFilter<TEntity>> filters)
        {
            Parameters = parameters;
            Filters = filters;
        }

        public IOltGenericParameter Parameters { get; }
        public List<IOltGenericFilter<TEntity>> Filters { get; }


        /// <summary>
        /// Called if filters are empty
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        protected abstract IQueryable<TEntity> DefaultFilter(IQueryable<TEntity> queryable);

        public override IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable)
        {

            var hasFilter = false;
            Filters.ForEach(filter =>
            {
                if (filter.HasValue(Parameters))
                {
                    hasFilter = true;
                    queryable = filter.BuildQueryable(queryable);
                }
            });

            return hasFilter ? queryable : DefaultFilter(queryable);

        }

    }
}
