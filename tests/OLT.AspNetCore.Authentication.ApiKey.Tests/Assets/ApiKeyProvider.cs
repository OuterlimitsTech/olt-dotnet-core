using Microsoft.Extensions.Logging;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets
{

    public class ApiKeyProvider : OltApiKeyProvider<ApiKeyService>
    {
        public ApiKeyProvider(ApiKeyService service, ILogger<ApiKeyService> logger) : base(service, logger)
        {
        }
    }
}
