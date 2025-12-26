using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Linq;
using System.Text;

namespace OLT.AspNetCore.Tests.Assets
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }


        public virtual void ConfigureServices(IServiceCollection services)
        {
            //OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services);
            services.AddRouting();            
        }


        public virtual void Configure(IApplicationBuilder app)
        {            
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}
