using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.PagedAdapterTests.Adapters;
using System;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.Tests.PagedAdapterTests
{
    public class AdapterTests : BaseAdpaterTests
    {
        private readonly Func<IQueryable<AdapterObject1>, IQueryable<AdapterObject1>> _defaultOrder = entity => entity.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

        [Fact]
        public void Map()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();                
                var adapter = new AdapterObject1ToAdapterObject2PagedAdapter();
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

        [Fact]
        public void MapPaged()
        {
            using (var provider = BuildProvider())
            {
                
                var adapter = new AdapterObject1ToAdapterObject2PagedAdapter();
                var obj1List = AdapterObject1.FakerList(23);
                var queryable = obj1List.AsQueryable();

                var resultQueryable = adapter.Map(queryable);
                Assert.Equal(obj1List.Count, resultQueryable.Count());

                var orderedExpected = _defaultOrder(queryable).ToList();
                var orderedResult = adapter.DefaultOrderBy(queryable).ToList();
                orderedResult.Should().BeEquivalentTo(orderedExpected);

            }
        }
    }


}
