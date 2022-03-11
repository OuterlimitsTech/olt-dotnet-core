using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters
{
    public class AdapterObject1ToAdapterObject2QueryableAdapter : OltAdapter<AdapterObject1, AdapterObject2>, IOltAdapterQueryable<AdapterObject1, AdapterObject2>
    {
        public override void Map(AdapterObject1 source, AdapterObject2 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(AdapterObject2 source, AdapterObject1 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdapterObject2> Map(IQueryable<AdapterObject1> queryable)
        {
            return queryable.Select(entity => new AdapterObject2
            {
                Name = new OltPersonName
                {
                    First = entity.FirstName,
                    Last = entity.LastName,
                }
            });
        }
    }

    public class AdapterObject2ToAdapterObject3QueryableAdapter : OltAdapter<AdapterObject2, AdapterObject3>, IOltAdapterQueryable<AdapterObject2, AdapterObject3>
    {
        public override void Map(AdapterObject2 source, AdapterObject3 destination)
        {
            throw new NotImplementedException();
        }

        public override void Map(AdapterObject3 source, AdapterObject2 destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdapterObject3> Map(IQueryable<AdapterObject2> queryable)
        {
            return queryable.Select(entity => new AdapterObject3
            {
                First = entity.Name.First,
                Last = entity.Name.Last,
            });
        }
    }
}
