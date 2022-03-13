using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Linq;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject3ToAdapterObject6QueryableAdapter : OltAdapter<AdapterObject3, AdapterObject6>, IOltAdapterQueryable<AdapterObject3, AdapterObject6>
    {
        public override void Map(AdapterObject3 source, AdapterObject6 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(AdapterObject6 source, AdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdapterObject6> Map(IQueryable<AdapterObject3> queryable)
        {
            throw new NotImplementedException();
        }
    }
}
