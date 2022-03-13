using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public abstract class BaseAdpaterTests
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();
            services.AddSingleton<IOltAdapter, AdapterObject1ToAdapterObject2Adapter>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3Adapter>();
            return services.BuildServiceProvider();
        }
    }


}
