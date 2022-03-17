using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.PagedAdapterTests.Models;
using System.Linq;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
    public class AdapterObject3ToAdapterObject1Adapter : OltAdapter<PagedAdapterObject3, PagedAdapterObject1>, IOltAdapterQueryable<PagedAdapterObject3, PagedAdapterObject1>
    {
        public override void Map(PagedAdapterObject3 source, PagedAdapterObject1 destination)
        {
            destination.FirstName = source.First;
            destination.LastName = source.Last;
        }

        public override void Map(PagedAdapterObject1 source, PagedAdapterObject3 destination)
        {
            destination.First = source.FirstName;
            destination.Last = source.LastName;
        }

        public IQueryable<PagedAdapterObject1> Map(IQueryable<PagedAdapterObject3> queryable)
        {
            return queryable.Select(entity => new PagedAdapterObject1 { FirstName = entity.First, LastName = entity.Last });
        }
    }
}