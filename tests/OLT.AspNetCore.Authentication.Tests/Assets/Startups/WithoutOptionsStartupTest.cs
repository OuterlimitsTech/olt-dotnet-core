using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Authentication.Tests.Assets;
using OLT.AspNetCore.Authentication.Tests.Assets.ApiKey;
using OLT.Core;

namespace OLT.AspNetCore.Authentication.Tests.Assets.Startups
{

    public class WithoutOptionsStartupTest : BaseStartup
    {
        public WithoutOptionsStartupTest(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddJwtBearer(JwtTokenTestExts.GetOptions());
            services.AddApiKey(new OltAuthenticationApiKey<ApiKeyProvider>(ApiKeyConstants.Realm, OltApiKeyLocation.HeaderOrQueryParams));
        }
    }
}
