using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject2ToAdapterObject3QueryableAdapter : OltAdapter<QueryableAdapterObject2, QueryableAdapterObject3>, IOltAdapterQueryable<QueryableAdapterObject2, QueryableAdapterObject3>
    {
        public override void Map(QueryableAdapterObject2 source, QueryableAdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(QueryableAdapterObject3 source, QueryableAdapterObject2 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<QueryableAdapterObject3> Map(IQueryable<QueryableAdapterObject2> queryable)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return queryable.Select(entity => new QueryableAdapterObject3
            {
                First = entity.Name.First,
                Last = entity.Name.Last,
            });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}