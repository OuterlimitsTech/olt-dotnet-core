using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets.Startups
{
    public abstract class BaseJwtTokenStartup
    {
        protected BaseJwtTokenStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        public virtual void Configure(IApplicationBuilder app)
        {        
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddControllers();
        }
    }
}
