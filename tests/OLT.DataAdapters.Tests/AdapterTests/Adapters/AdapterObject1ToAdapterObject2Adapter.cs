using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public class AdapterObject1ToAdapterObject2Adapter : OltAdapter<AdapterObject1, AdapterObject2>
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
    }
}
