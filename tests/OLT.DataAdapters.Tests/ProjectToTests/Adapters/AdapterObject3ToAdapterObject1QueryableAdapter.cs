using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{

    public class AdapterObject3ToAdapterObject1QueryableAdapter : OltAdapter<AdapterObject3, AdapterObject1>, IOltAdapterQueryable<AdapterObject3, AdapterObject1>
    {
        public override void Map(AdapterObject1 source, AdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(AdapterObject3 source, AdapterObject1 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdapterObject1> Map(IQueryable<AdapterObject3> queryable)
        {
            return queryable.Select(entity => new AdapterObject1
            {
                FirstName = entity.First,
                LastName = entity.Last,
            });
        }
    }
}
