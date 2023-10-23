using AspNetCore.Authentication.ApiKey;
using System.Collections.Generic;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets
{
    public class ApiKeyResult : IApiKey
    {
        public ApiKeyResult(List<System.Security.Claims.Claim> claims)
        {
            Claims = claims;
        }

        public string Key { get; set; }
        public string OwnerName { get; set; }
        public IReadOnlyCollection<System.Security.Claims.Claim> Claims { get; }
    }
}
