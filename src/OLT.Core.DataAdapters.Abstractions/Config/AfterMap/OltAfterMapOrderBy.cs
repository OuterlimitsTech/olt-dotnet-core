using System;
using System.Linq;

namespace OLT.Core
{
    public class OltAfterMapOrderBy<TSource, TDestination> : OltAdapterAfterMap<TSource, TDestination>
    {
        public OltAfterMapOrderBy(Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> orderBy)
        {
            _orderBy = orderBy;
        }

        private readonly Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> _orderBy;

        public override IQueryable<TDestination> AfterMap(IQueryable<TDestination> queryable)
        {
            return _orderBy(queryable);
        }
    }
}