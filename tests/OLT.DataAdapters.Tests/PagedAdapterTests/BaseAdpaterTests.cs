using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.PagedAdapterTests.Adapters;

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
            return services.BuildServiceProvider();
        }
    }


}
