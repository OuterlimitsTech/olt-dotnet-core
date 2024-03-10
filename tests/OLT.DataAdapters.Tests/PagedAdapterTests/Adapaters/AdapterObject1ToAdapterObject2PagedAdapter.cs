using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.PagedAdapterTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class AdapterObject1ToAdapterObject2PagedAdapter : OltAdapterPaged<PagedAdapterObject1, PagedAdapterObject2>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public override void Map(PagedAdapterObject1 source, PagedAdapterObject2 destination)
        {
            destination.Name = new OltPersonName
            {
                First = source.FirstName,
                Last = source.LastName,
            };
        }

        public override void Map(PagedAdapterObject2 source, PagedAdapterObject1 destination)
        {
            destination.FirstName = source.Name?.First;
            destination.LastName = source.Name?.Last;
        }

        public override IQueryable<PagedAdapterObject2> Map(IQueryable<PagedAdapterObject1> queryable)
        {
            return queryable.Select(entity => new PagedAdapterObject2
            {
                Name = new OltPersonName { First = entity.FirstName, Last = entity.LastName },
            });
        }

        public override IOrderedQueryable<PagedAdapterObject1> DefaultOrderBy(IQueryable<PagedAdapterObject1> queryable)
        {
            return queryable.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
        }
    }
}
