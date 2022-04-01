﻿using System.Collections.Generic;
using System.Linq;

namespace OLT.Core.Searchers.Tests.Assets
{
    public class FakeEntityGenericFilterSearcher : OltGenericFilterSearcher<FakeEntity>
    {
        protected FakeEntityGenericFilterSearcher(IOltGenericParameter parameters, List<IOltGenericFilter<FakeEntity>> filters) : base(parameters, filters)
        {
        }

        protected override IQueryable<FakeEntity> DefaultFilter(IQueryable<FakeEntity> queryable)
        {
            return queryable.Where(p => p.DeletedOn == null);
        }
    }
}