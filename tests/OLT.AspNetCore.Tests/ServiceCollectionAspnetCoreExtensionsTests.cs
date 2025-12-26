using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;

namespace OLT.AspNetCore.Tests
{

    public class ServiceCollectionAspnetCoreExtensionsTests
    {
        private static string RunEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        [Fact]
        public void HostServiceTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var hostService = testServer.Services.GetRequiredService<IOltHostService>();
                Assert.NotNull(hostService);
                var result = hostService.ResolveRelativePath("~/test");
                Assert.Equal(Path.Combine(AppContext.BaseDirectory, "test"), result);
                Assert.Equal("OLT.AspNetCore.Tests", hostService.ApplicationName);
                Assert.Equal(RunEnvironment, hostService.EnvironmentName);                
            }
        }

        [Fact]
        public void IdentityAnonymousTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {                
                var identity = testServer.Services.GetRequiredService<IOltIdentity>();
                Assert.NotNull(identity);
                Assert.True(identity.IsAnonymous);
                var dbAuditUser = testServer.Services.GetRequiredService<IOltDbAuditUser>();
                Assert.NotNull(dbAuditUser);
                Assert.Null(dbAuditUser.GetDbUsername());                
            }
        }

      
      
    }
}