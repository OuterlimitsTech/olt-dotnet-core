using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{
    [Obsolete("Being Removed in 9.x -> This class provides no value")]
    public class OltIdentityAspNetCore : OltIdentity
    {
        private readonly IHttpContextAccessor _httpContext;

        public OltIdentityAspNetCore(IHttpContextAccessor httpContext)
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