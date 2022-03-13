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
        public AdapterObject1ToAdapterObject2QueryableAdapter()
        {
            this.BeforeMap(p => p.OrderBy(o => o.LastName).ThenBy(o => o.FirstName));
        }

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
}
