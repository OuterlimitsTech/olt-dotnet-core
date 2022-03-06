using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets.Startups
{

    public abstract class BaseApiKeyStartup
    {
        protected BaseApiKeyStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public void DefaultServices(IServiceCollection services)
        {

            services
                .AddScoped<IOltApiKeyProvider, ApiKeyProvider>()
                .AddScoped<IApiKeyProvider>(opt => opt.GetRequiredService<IOltApiKeyProvider>())
                .AddScoped<ApiKeyService>()
                .AddScoped<IOltApiKeyService>(opt => opt.GetRequiredService<ApiKeyService>());
        }
    }
}
