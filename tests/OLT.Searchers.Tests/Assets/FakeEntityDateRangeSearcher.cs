using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Searchers.Tests.Assets
{
    public class FakeEntityDateRangeSearcher : OltSearcherDateRange<FakeEntity>
    {
        public FakeEntityDateRangeSearcher() : base() 
        { 
        }

        public FakeEntityDateRangeSearcher(OltDateRange value) : base(value) 
        { 
        }

        public FakeEntityDateRangeSearcher(DateTimeOffset start, DateTimeOffset end) : base(start, end) 
        { 
        }


        public DateTimeOffset QueryEndValue => QueryEnd;

        public override IQueryable<FakeEntity> BuildQueryable(IQueryable<FakeEntity> queryable)
        {
            return queryable.Where(p => p.SomeDate >= Value.Start && p.SomeDate < QueryEnd);
        }
    }
}
