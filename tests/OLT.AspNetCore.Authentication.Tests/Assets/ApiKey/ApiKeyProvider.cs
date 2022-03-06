using Microsoft.Extensions.Logging;

namespace OLT.AspNetCore.Authentication.Tests.Assets.ApiKey
{
    public class ApiKeyProvider : OltApiKeyProvider<ApiKeyService>
    {
        public ApiKeyProvider(ApiKeyService service, ILogger<ApiKeyService> logger) : base(service, logger)
        {
        }
    }

}
