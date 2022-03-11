////using Microsoft.Extensions.DependencyInjection;
////using Microsoft.Extensions.Hosting;
////using OLT.Core;
////using OLT.DataAdapters.Tests.AdapterTests;

////namespace OLT.DataAdapters.Tests
////{
////    internal class Startup
////    {

////        public void ConfigureHost(IHostBuilder hostBuilder)
////        {
////            //Nothing to do here


////            //hostBuilder.ConfigureHostConfiguration(builder =>
////            //{
////            //    builder.AddEnvironmentVariables();
////            //});
////        }

////        public virtual void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
////        {
////            services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();
////            services.AddSingleton<IOltAdapter, ObjectAdapter>();


////            var resolverTest = services.BuildServiceProvider().GetRequiredService<IOltAdapterResolver>();
////        }
////    }
////}
