using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.DataAdapters.Tests.ProjectToTests
{
    public abstract class BaseAdpaterTests : BaseAdapterTests
    {
        protected override ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();
            services.AddSingleton<IOltAdapter, AdapterObject1ToAdapterObject2QueryableAdapter>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3QueryableAdapter>();
            return services.BuildServiceProvider();
        }
    }

    public partial class AdapterTests : BaseAdpaterTests
    {
        [Fact]
        public void Map()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();

                Assert.Throws<NotImplementedException>(() => adapter.Map(AdapterObject1.FakerData(), new AdapterObject2()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<AdapterObject1, AdapterObject2>(AdapterObject1.FakerData(), new AdapterObject2()));

                Assert.Throws<NotImplementedException>(() => adapter.Map(AdapterObject2.FakerData(), new AdapterObject1()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<AdapterObject2, AdapterObject1>(AdapterObject2.FakerData(), new AdapterObject1()));
            }
        }


        [Fact]
        public void ProjectTo()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();
                var queryableObj1 = AdapterObject1.FakerList(3).AsQueryable();

                var obj2Result = adapter.Map(queryableObj1).ToList(); 
                adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(queryableObj1).Should().BeEquivalentTo(obj2Result);

            }
        }
    }
}
