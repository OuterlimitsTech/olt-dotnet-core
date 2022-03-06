using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.Tests.Assets
{
    public static class JwtTokenTestExts
    {
        public const string Authority = "local_Authority";
        public const string Audience = "local_Audience";

        public static OltAuthenticationJwtBearer GetOptions()
        {
            return new OltAuthenticationJwtBearer
            {
                JwtSecret = "ABC1234",
                RequireHttpsMetadata = false,
                ValidateIssuer = true,
                ValidateAudience = true,
            };
        }
    }
}
