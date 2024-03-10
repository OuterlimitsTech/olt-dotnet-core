using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;
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
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();                
                var adapter = new AdapterObject1ToAdapterObject2Adapter();
                var obj1 = BasicAdapterObject1.FakerData();

                var resultObj2 = new BasicAdapterObject2();
                adapter.Map(obj1, resultObj2);
                adapterResolver.Map(obj1, new BasicAdapterObject2()).Should().BeEquivalentTo(resultObj2);

                var resultObj1 = new BasicAdapterObject1();
                adapter.Map(resultObj2, resultObj1);
                resultObj1.Should().BeEquivalentTo(obj1);
                adapterResolver.Map(resultObj2, new BasicAdapterObject1()).Should().BeEquivalentTo(resultObj1);               

            }
        }
    }


}
