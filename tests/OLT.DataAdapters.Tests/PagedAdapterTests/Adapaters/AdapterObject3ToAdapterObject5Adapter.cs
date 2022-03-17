using OLT.Core;
using OLT.DataAdapters.Tests.PagedAdapterTests.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
    public class AdapterObject3ToAdapterObject5Adapter : OltAdapter<PagedAdapterObject3, PagedAdapterObject5>
    {
        public override void Map(PagedAdapterObject3 source, PagedAdapterObject5 destination)
        {
            destination.FirstName = source.First;
            destination.LastName = source.Last;
        }

        public override void Map(PagedAdapterObject5 source, PagedAdapterObject3 destination)
        {
            destination.First = source.FirstName;
            destination.Last = source.LastName;
        }
    }
}