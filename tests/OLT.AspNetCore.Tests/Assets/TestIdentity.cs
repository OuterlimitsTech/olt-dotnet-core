using Microsoft.AspNetCore.Http;
using OLT.Core;

namespace OLT.AspNetCore.Tests.Assets
{
    public class TestIdentity : OltIdentity
    {
        private readonly IHttpContextAccessor _httpContext;

        public TestIdentity(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public override System.Security.Claims.ClaimsPrincipal? Identity => _httpContext.HttpContext?.User;

        public override bool HasRole<TRoleEnum>(params TRoleEnum[] roles)
        {
            return roles?.Any(role => HasRole(role.GetCodeEnum())) == true;
        }
    }

}
