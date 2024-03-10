using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.DataAdapters.Tests.ProjectToTests
{
    public abstract class BaseAdpaterTests 
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
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
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();

                Assert.Throws<NotImplementedException>(() => adapter.Map(QueryableAdapterObject1.FakerData(), new QueryableAdapterObject2()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject1, QueryableAdapterObject2>(QueryableAdapterObject1.FakerData(), new QueryableAdapterObject2()));

                Assert.Throws<NotImplementedException>(() => adapter.Map(QueryableAdapterObject2.FakerData(), new QueryableAdapterObject1()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject2, QueryableAdapterObject1>(QueryableAdapterObject2.FakerData(), new QueryableAdapterObject1()));
            }
        }


        [Fact]
        public void ProjectTo()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();
                var queryableObj1 = QueryableAdapterObject1.FakerList(3).AsQueryable();

                var obj2Result = adapter.Map(queryableObj1).ToList(); 
                adapterResolver.ProjectTo<QueryableAdapterObject1, QueryableAdapterObject2>(queryableObj1).Should().BeEquivalentTo(obj2Result);

            }
        }
    }
}
