using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Tests.Assets
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services                
                .AddSingleton<IOltHostService, OltHostAspNetCoreService>()
                .AddScoped<IOltIdentity, OltIdentityAspNetCore>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>())
                .AddHttpContextAccessor();

            services.AddRouting();
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app)
        {            
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}
