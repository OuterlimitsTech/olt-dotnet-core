using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.AspNetCore.Tests.Assets
{
    public class StartupWithApiVersion : Startup
    {
        public StartupWithApiVersion(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            OltServiceCollectionAspnetCoreExtensions.AddApiVersioning(services, new OltOptionsApiVersion());
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}
