using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public class AdapterObject1ToAdapterObject2Adapter : OltAdapter<BasicAdapterObject1, BasicAdapterObject2>
    {
        public override void Map(BasicAdapterObject1 source, BasicAdapterObject2 destination)
        {
            destination.Name = new OltPersonName
            {
                First = source.FirstName,
                Last = source.LastName,
            };
        }

        public override void Map(BasicAdapterObject2 source, BasicAdapterObject1 destination)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            destination.FirstName = source.Name.First;
            destination.LastName = source.Name.Last;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
