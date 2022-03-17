using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject3ToAdapterObject6QueryableAdapter : OltAdapter<QueryableAdapterObject3, QueryableAdapterObject6>, IOltAdapterQueryable<QueryableAdapterObject3, QueryableAdapterObject6>
    {
        public override void Map(QueryableAdapterObject3 source, QueryableAdapterObject6 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(QueryableAdapterObject6 source, QueryableAdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QueryableAdapterObject6> Map(IQueryable<QueryableAdapterObject3> queryable)
        {
            throw new NotImplementedException();
        }
    }
}
