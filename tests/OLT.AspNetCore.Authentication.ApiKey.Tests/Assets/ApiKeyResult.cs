using AspNetCore.Authentication.ApiKey;
using System.Collections.Generic;
using System.Security.Claims;

namespace OLT.AspNetCore.Authentication.ApiKey.Tests.Assets
{
    public class ApiKeyResult : IApiKey
    {
        public ApiKeyResult(List<Claim> claims)
        {
            Claims = claims;
        }

        public string Key { get; set; }
        public string OwnerName { get; set; }
        public IReadOnlyCollection<Claim> Claims { get; }
    }
}
