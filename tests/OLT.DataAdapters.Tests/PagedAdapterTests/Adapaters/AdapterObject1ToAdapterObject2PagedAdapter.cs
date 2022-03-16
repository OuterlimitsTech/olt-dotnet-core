using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class AdapterObject1ToAdapterObject2PagedAdapter : OltAdapterPaged<AdapterObject1, AdapterObject2>
#pragma warning restore CS0618 // Type or member is obsolete
    {

        public override void Map(AdapterObject1 source, AdapterObject2 destination)
        {
            destination.Name = new OltPersonName
            {
                First = source.FirstName,
                Last = source.LastName,
            };
        }

        public override void Map(AdapterObject2 source, AdapterObject1 destination)
        {
            destination.FirstName = source.Name.First;
            destination.LastName = source.Name.Last;
        }

        public override IQueryable<AdapterObject2> Map(IQueryable<AdapterObject1> queryable)
        {
            return queryable.Select(entity => new AdapterObject2
            {
                Name = new OltPersonName { First = entity.FirstName, Last = entity.LastName },
            });
        }

        public override IOrderedQueryable<AdapterObject1> DefaultOrderBy(IQueryable<AdapterObject1> queryable)
        {
            return queryable.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
        }
    }
}
