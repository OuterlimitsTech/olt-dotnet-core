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
        public BaseJwtTokenStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app) //, IWebHostEnvironment env, IOptions<AppSettingsDto> options)
        {
            //var settings = options.Value;
            //app.UsePathBase(settings.Hosting);
            //app.UseDeveloperExceptionPage(settings.Hosting);
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});
            //app.UseHsts(settings.Hosting);
            //app.UseCors(settings.Hosting);
            //app.UseHttpsRedirection(settings.Hosting);
            app.UseAuthentication();
            //app.UseOltSerilogRequestLogging();
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void DefaultServices(IServiceCollection services)
        {
            //var settings = Configuration.GetSection("AppSettings").Get<AppSettingsDto>();

            //services
            //    .AddOltUnitTesting(this.GetType().Assembly)
            //    .AddOltAspNetCore(settings, this.GetType().Assembly, null);
        }
    }
}
