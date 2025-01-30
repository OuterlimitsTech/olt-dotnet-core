using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OLT.Identity.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{

    public abstract class OltRequirePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {

        public abstract List<string> RoleClaims { get; }


        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true && context.HttpContext.User.FindAll(ClaimTypeNames.Role).Any(p => RoleClaims.Contains(p.Value)))
            {
                return;
            }
            context.Result = new UnauthorizedResult();
        }

    }
}