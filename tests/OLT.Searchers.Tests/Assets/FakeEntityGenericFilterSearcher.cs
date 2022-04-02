using OLT.Core;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Searchers.Tests.Assets
{
    public class FakeEntityGenericFilterSearcher : OltGenericFilterSearcher<FakeEntity>
    {
        public FakeEntityGenericFilterSearcher(IOltGenericParameter parameters, List<IOltGenericFilter<FakeEntity>> filters) : base(parameters, filters)
        {
        }

        protected override IQueryable<FakeEntity> DefaultFilter(IQueryable<FakeEntity> queryable)
        {
            return queryable.Where(p => p.DeletedOn == null);
        }
    }
}
