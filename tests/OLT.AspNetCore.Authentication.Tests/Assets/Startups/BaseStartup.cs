using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Authentication.Tests.Assets.ApiKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.Tests.Assets.Startups
{
    public abstract class BaseStartup
    {
        protected BaseStartup(IConfiguration configuration)
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

            services
                .AddScoped<IOltApiKeyProvider, ApiKeyProvider>()
                .AddScoped<IApiKeyProvider>(opt => opt.GetRequiredService<IOltApiKeyProvider>())
                .AddScoped<ApiKeyService>()
                .AddScoped<IOltApiKeyService>(opt => opt.GetRequiredService<ApiKeyService>());

            services.AddRouting();
            services.AddControllers();
        }
    }
}
