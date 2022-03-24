using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using OLT.AspNetCore.Tests.Assets;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class RequirePermissionAttributeTests
    {

        [Fact]
        public async Task NoIdentityTest()
        {
            var actionFilter = new RequirePermissionAttribute(SecurityPermissions.ReadOnly);
            var filters = new List<IFilterMetadata>();
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, filters);
            actionFilter.OnAuthorization(authorizationFilterContext);
            Assert.NotNull(authorizationFilterContext.Result as UnauthorizedResult);

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<StartupWithAuth>()))
            {
                using (var client = testServer.CreateClient())
                {
                    var response = await client.GetAsync("/api/permissions-test");
                    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                }

                using (var client = testServer.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", "XYZ");
                    var response = await client.GetAsync("/api/permissions-test");
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }

        [Fact]
        public void WithInvalidRole()
        {
            var actionFilter = new RequirePermissionAttribute(SecurityPermissions.ReadOnly);
            var filters = new List<IFilterMetadata>();
            var httpContext = new DefaultHttpContext();
            IPrincipal principal = new GenericPrincipal(new GenericIdentity("TestName"), new[] { "update-data" });
            httpContext.User = new System.Security.Claims.ClaimsPrincipal(principal);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, filters);
            actionFilter.OnAuthorization(authorizationFilterContext);
            Assert.NotNull(authorizationFilterContext.Result as UnauthorizedResult);
        }

        [Fact]
        public void WithValidRole()
        {
            var actionFilter = new RequirePermissionAttribute(SecurityPermissions.ReadOnly);
            var filters = new List<IFilterMetadata>();
            var httpContext = new DefaultHttpContext();
            IPrincipal principal = new GenericPrincipal(new GenericIdentity("TestName"), new[] { "read-only" });
            httpContext.User = new System.Security.Claims.ClaimsPrincipal(principal);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, filters);
            actionFilter.OnAuthorization(authorizationFilterContext);
            Assert.Null(authorizationFilterContext.Result);
        }

     
    }
}