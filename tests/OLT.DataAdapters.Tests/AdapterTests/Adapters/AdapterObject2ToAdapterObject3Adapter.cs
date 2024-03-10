using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;
using OLT.DataAdapters.Tests.Assets.Models;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public class AdapterObject2ToAdapterObject3Adapter : OltAdapter<BasicAdapterObject2, BasicAdapterObject3>
    {
        public override void Map(BasicAdapterObject2 source, BasicAdapterObject3 destination)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            destination.First = source.Name.First;
            destination.Last = source.Name.Last;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public override void Map(BasicAdapterObject3 source, BasicAdapterObject2 destination)
        {
            destination.Name = new OltPersonName();
            destination.Name.First = source.First;
            destination.Name.Last = source.Last;
        }
    }
}
