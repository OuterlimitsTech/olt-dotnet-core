using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using OLT.AspNetCore.Tests.Assets;
using System.Net;
using System.Security.Principal;

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

            using var host = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                        .UseTestServer() // If using TestServer
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<StartupWithAuth>()
                        ;
                })
                .Build();
            await host.StartAsync();

            using var testServer = host.GetTestServer();
            using var client = testServer.CreateClient();
            var response = await client.GetAsync("/api/permissions-test");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }

        [Fact]
        public async Task WithInvalidRole()
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

            using var host = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                        .UseTestServer() // If using TestServer
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<StartupWithAuth>()
                        ;
                })
                .Build();
            await host.StartAsync();

            using var testServer = host.GetTestServer();
            using var client = testServer.CreateClient();
            client.DefaultRequestHeaders.Add("X-API-Key", "123");
            var response = await client.GetAsync("/api/permissions-test");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }

        [Fact]
        public async Task WithValidRole()
        {
            var actionFilter = new RequirePermissionAttribute(SecurityPermissions.ReadOnly);
            var filters = new List<IFilterMetadata>();
            var httpContext = new DefaultHttpContext();
            IPrincipal principal = new GenericPrincipal(new GenericIdentity("TestName"), new[] { "read-only" });
            httpContext.User = new System.Security.Claims.ClaimsPrincipal(principal);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary());
            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, filters);
            actionFilter.OnAuthorization(authorizationFilterContext);
            //Assert.Null(authorizationFilterContext.Result);
            Assert.NotNull(authorizationFilterContext.Result as UnauthorizedResult);


            using var host = new HostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                        .UseTestServer() // If using TestServer
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<StartupWithAuth>()
                        ;
                })
                .Build();
            await host.StartAsync();

            using var testServer = host.GetTestServer();
            using var client = testServer.CreateClient();
            client.DefaultRequestHeaders.Add("X-API-Key", "XYZ");
            var response = await client.GetAsync("/api/permissions-test");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

     
    }
}