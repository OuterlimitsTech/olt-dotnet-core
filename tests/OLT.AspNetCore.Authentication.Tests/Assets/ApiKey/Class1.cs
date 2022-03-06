using AspNetCore.Authentication.ApiKey;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.Tests.Assets.ApiKey
{
    public static class ApiKeyConstants
    {
        public const string Key1 = "1234";
        public const string Realm = "API Key Unit Test";
    }

    //public static class ApiKeyTestExts
    //{
    //    public const string Authority = "local_Authority";
    //    public const string Audience = "local_Audience";

    //    public static OltAuthenticationapi GetOptions()
    //    {
    //        return new OltAuthenticationJwtBearer
    //        {
    //            JwtSecret = "ABC1234",
    //            RequireHttpsMetadata = false,
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //        };
    //    }
    //}

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

    public class ApiKeyService : OltDisposable, IOltApiKeyService
    {
        public Task<IApiKey> ValidateAsync(string key)
        {
            var ownerName = "Test Api Key";

            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, ownerName, ClaimValueTypes.String)
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
