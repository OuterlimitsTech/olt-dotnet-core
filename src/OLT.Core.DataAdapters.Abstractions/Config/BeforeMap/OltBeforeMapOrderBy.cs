using System;
using System.Linq;

namespace OLT.Core
{
    public class OltBeforeMapOrderBy<TSource, TDestination> : OltAdapterBeforeMap<TSource, TDestination>
    {
        public OltBeforeMapOrderBy(Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy)
        {
            _orderBy = orderBy;
        }

        private readonly Func<IQueryable<TSource>, IOrderedQueryable<TSource>> _orderBy;

        public override IQueryable<TSource> BeforeMap(IQueryable<TSource> queryable)
        {
            return _orderBy(queryable);
        }
    }

}