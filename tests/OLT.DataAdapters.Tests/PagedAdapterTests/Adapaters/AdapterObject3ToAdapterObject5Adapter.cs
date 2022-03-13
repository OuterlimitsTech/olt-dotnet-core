using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Adapters
{
    public class AdapterObject3ToAdapterObject5Adapter : OltAdapter<AdapterObject3, AdapterObject5>
    {
        public override void Map(AdapterObject3 source, AdapterObject5 destination)
        {
            destination.FirstName = source.First;
            destination.LastName = source.Last;
        }

        public override void Map(AdapterObject5 source, AdapterObject3 destination)
        {
            {
                destination.First = source.FirstName;
                destination.Last = source.LastName;
            }
        }


    }
}