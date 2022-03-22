using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            //services.AddScoped<IOltIdentity, TestIdentity>().AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());

            services.AddRouting();
            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app)
        {
            //app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}
