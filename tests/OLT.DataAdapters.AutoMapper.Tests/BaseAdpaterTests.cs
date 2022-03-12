using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public abstract class BaseAdpaterTests
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3Adapter>();
            services.AddAutoMapper(this.GetType().Assembly);
            return services.BuildServiceProvider();
        }
    }


}
