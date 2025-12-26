using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;


namespace OLT.AspNetCore.Tests
{

    public class ServiceCollectionAspnetCoreExtensionsTests
    {
        private static string RunEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        [Fact]
        public async Task HostServiceTests()
        {
            using var host = new HostBuilder()                
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder                    
                        .UseTestServer() // If using TestServer
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseWebRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>()
                        ;
                })
                .Build();
            await host.StartAsync();

            using var testServer = host.GetTestServer();
            var hostService = testServer.Services.GetRequiredService<IOltHostService>();
            Assert.NotNull(hostService);
            Assert.Equal("OLT.AspNetCore.Tests", hostService.ApplicationName);
            Assert.Equal(RunEnvironment, hostService.EnvironmentName);
            var result = hostService.ResolveRelativePath("~/test");
            Assert.Equal(Path.Combine(AppContext.BaseDirectory, "test"), result);

        }

        [Fact]
        public async Task IdentityAnonymousTests()
        {
            using var host = new HostBuilder()
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder
                    .UseTestServer() // If using TestServer
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseWebRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    ;
            })
            .Build();
            await host.StartAsync();

            using var testServer = host.GetTestServer();
            var identity = testServer.Services.GetRequiredService<IOltIdentity>();
            Assert.NotNull(identity);
            Assert.True(identity.IsAnonymous);
            var dbAuditUser = testServer.Services.GetRequiredService<IOltDbAuditUser>();
            Assert.NotNull(dbAuditUser);
            Assert.Null(dbAuditUser.GetDbUsername());
        }

      
      
    }
}