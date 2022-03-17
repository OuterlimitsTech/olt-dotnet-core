using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject3ToAdapterObject1QueryableAdapter : OltAdapter<QueryableAdapterObject3, QueryableAdapterObject1>, IOltAdapterQueryable<QueryableAdapterObject3, QueryableAdapterObject1>
    {
        public override void Map(QueryableAdapterObject1 source, QueryableAdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(QueryableAdapterObject3 source, QueryableAdapterObject1 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QueryableAdapterObject1> Map(IQueryable<QueryableAdapterObject3> queryable)
        {
            return queryable.Select(entity => new QueryableAdapterObject1
            {
                FirstName = entity.First,
                LastName = entity.Last,
            });
        }
    }
}
