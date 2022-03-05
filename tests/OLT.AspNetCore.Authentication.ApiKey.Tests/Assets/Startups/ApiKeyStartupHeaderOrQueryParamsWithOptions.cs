using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets.Startups
{
    public class ApiKeyStartupHeaderOrQueryParamsWithOptions : BaseApiKeyStartup
    {
        public ApiKeyStartupHeaderOrQueryParamsWithOptions(IConfiguration configuration) : base(configuration) { }

        public void ConfigureServices(IServiceCollection services)
        {
            base.DefaultServices(services);
            services.AddApiKey(new OltAuthenticationApiKey<ApiKeyProvider>(ApiKeyConstants.Realm, OltApiKeyLocation.HeaderOrQueryParams), opt => opt.IgnoreAuthenticationIfAllowAnonymous = false);
        }
    }
}
