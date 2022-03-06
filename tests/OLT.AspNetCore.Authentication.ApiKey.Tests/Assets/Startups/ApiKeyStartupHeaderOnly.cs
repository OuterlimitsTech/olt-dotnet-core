using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets.Startups
{
    public class ApiKeyStartupHeaderOnly : BaseApiKeyStartup
    {
        public ApiKeyStartupHeaderOnly(IConfiguration configuration) : base(configuration) { }

        public void ConfigureServices(IServiceCollection services)
        {
            base.DefaultServices(services);
            services.AddApiKey(new OltAuthenticationApiKey<ApiKeyProvider>(ApiKeyConstants.Realm, OltApiKeyLocation.HeaderOnly));
        }
    }
}
