using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using System.Collections.Generic;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public abstract class BaseAdpaterTests
    {
        private List<Profile> DefaultMaps = new List<Profile> { new TestMaps(), new AdapterObject4PagedMap() };

        protected void RegisterMaps(IServiceCollection services, List<Profile> maps)
        {
            services.AddAutoMapper(cfg =>
            {
                maps.ForEach(map => cfg.AddProfile(map));
            });
        }

        protected ServiceProvider BuildProvider(List<Profile> maps = null)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3Adapter>();
            services.AddSingleton<IOltAdapter, AdapterObject4PagedMap>();
            RegisterMaps(services, maps ?? DefaultMaps);
            //services.AddAutoMapper(this.GetType().Assembly);
            //services.AddAutoMapper(cfg =>
            //{
            //    if (maps != null)
            //    {
            //        maps.ForEach(map => cfg.AddProfile(map));
            //    }
            //    else
            //    {
            //        cfg.AddProfile(new TestMaps());
            //        cfg.AddProfile(new AdapterObject4PagedMap());
            //    }
            //});
            return services.BuildServiceProvider();
        }
    }


}
