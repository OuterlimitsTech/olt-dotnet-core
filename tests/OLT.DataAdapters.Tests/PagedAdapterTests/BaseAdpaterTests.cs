using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.PagedAdapterTests.Adapters;
using System.Linq;

namespace OLT.DataAdapters.Tests.PagedAdapterTests
{
    public abstract class BaseAdpaterTests
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();
            services.AddSingleton<IOltAdapter, AdapterObject1ToAdapterObject2PagedAdapter>();
            services.AddSingleton<IOltAdapter, AdapterObject3ToAdapterObject1Adapter>();
            services.AddSingleton<IOltAdapter, AdapterObject3ToAdapterObject5Adapter>();

            OltAdapterMapConfigs.AfterMap.Register(new OltAfterMapOrderBy<AdapterObject3, AdapterObject1>(p => p.OrderBy(p => p.LastName).ThenBy(p => p.FirstName)), false);

            return services.BuildServiceProvider();
        }
    }


}
