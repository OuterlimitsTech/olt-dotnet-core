using Microsoft.AspNetCore.Http;
using System;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{
    [Obsolete("Being Removed in 9.x -> Reason for Class")]
    public class OltIdentityAspNetCore : OltIdentity
    {
        private readonly IHttpContextAccessor _httpContext;

        public OltIdentityAspNetCore(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public override System.Security.Claims.ClaimsPrincipal? Identity => _httpContext.HttpContext?.User;


    }
}