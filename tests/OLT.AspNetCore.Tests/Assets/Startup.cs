using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Utility.AssemblyScanner;

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
            var assemblies = new OltAssemblyScanBuilder()
                .IncludeFilter("OLT.")
                .ExcludeMicrosoft()
                .ExcludeAutomapper()
                .DeepScan()
                .Build();


            services.AddServicesFromAssemblies(builder => builder.IncludeAssemblies(assemblies));

            services.AddHttpContextAccessor();
           
            //services.AddSingleton<IOltHostService, OltHostAspNetCoreService>();
            services.AddScoped<IOltIdentity, TestIdentity>();
            services.AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());
            services.AddRouting();
            services.AddControllers();
        }


        public virtual void Configure(IApplicationBuilder app)
        {            
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }

}
