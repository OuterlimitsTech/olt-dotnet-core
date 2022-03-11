using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using Xunit;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public class AdapterTests : BaseAdpaterTests
    {
        [Fact]
        public void Map()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();                
                var adapter = new AdapterObject1ToAdapterObject2Adapter();
                var obj1 = AdapterObject1.FakerData();

                var resultObj2 = new AdapterObject2();
                adapter.Map(obj1, resultObj2);
                adapterResolver.Map(obj1, new AdapterObject2()).Should().BeEquivalentTo(resultObj2);

                var resultObj1 = new AdapterObject1();
                adapter.Map(resultObj2, resultObj1);
                resultObj1.Should().BeEquivalentTo(obj1);
                adapterResolver.Map(resultObj2, new AdapterObject1()).Should().BeEquivalentTo(resultObj1);               

            }
        }
    }


}
