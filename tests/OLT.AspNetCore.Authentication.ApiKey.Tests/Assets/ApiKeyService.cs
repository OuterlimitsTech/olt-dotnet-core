using AspNetCore.Authentication.ApiKey;
using OLT.Constants;
using OLT.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets
{
    public class ApiKeyService : OltDisposable, IOltApiKeyService
    {
        public Task<IApiKey> ValidateAsync(string key)
        {
            var ownerName = "Test Api Key";

            var claimsList = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(OltClaimTypes.PreferredUsername, ownerName, System.Security.Claims.ClaimValueTypes.String)
            };

            if (key == "1234")
            {
                var result = new ApiKeyResult(claimsList)
                {
                    Key = key,
                    OwnerName = ownerName
                };

                return Task.FromResult<IApiKey>(result);
            }


            return Task.FromException<IApiKey>(new OltException("Invalid Key"));

        }
    }
}
