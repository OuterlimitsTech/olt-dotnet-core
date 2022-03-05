using AspNetCore.Authentication.ApiKey;
using OLT.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets
{
    public class ApiKeyService : OltDisposable, IOltApiKeyService
    {
        public Task<IApiKey> ValidateAsync(string key)
        {
            var ownerName = "Test Api Key";

            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ownerName, ClaimValueTypes.String)
            };

            var result = new ApiKeyResult(claimsList)
            {
                Key = key,
                OwnerName = ownerName
            };


            return Task.FromResult<IApiKey>(result);
        }
    }
}
