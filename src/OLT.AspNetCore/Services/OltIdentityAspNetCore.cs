﻿using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{
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