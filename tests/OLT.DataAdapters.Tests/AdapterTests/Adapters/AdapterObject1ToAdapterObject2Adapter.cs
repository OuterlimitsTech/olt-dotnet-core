using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            destination.FirstName = source.Name.First;
            destination.LastName = source.Name.Last;
        }
    }
}
