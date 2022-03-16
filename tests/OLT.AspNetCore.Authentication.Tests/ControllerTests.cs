using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.AspNetCore.Authentication.Tests.Assets;
using OLT.AspNetCore.Authentication.Tests.Assets.Startups;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace OLT.AspNetCore.Authentication.Tests
{
    public class ControllerTests
    {

        private async Task ValidateConfig(TestServer testServer)
        {
            var services = testServer.Host.Services;
            var schemeProvider = services.GetRequiredService<IAuthenticationSchemeProvider>();
            Assert.NotNull(schemeProvider);

            var list = (await schemeProvider.GetAllSchemesAsync()).ToList();
            Assert.NotNull(list);


            var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
            Assert.NotNull(scheme);

        }

        private async Task ApiKeyAuthTest(TestServer testServer)
        {
            var request = testServer.CreateRequest("/api");

            var response = await request.SendAsync("GET");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            request = testServer.CreateRequest("/api");
            request.AddHeader("X-API-KEY", "1234");
            response = await request.SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ServerWithOptions()
        {
            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<WithOptionsStartupTest>()))
            {
                await ValidateConfig(testServer);
                await ApiKeyAuthTest(testServer);                
            }
        }

        [Fact]
        public async Task ServerWithoutOptions()
        {
            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<WithoutOptionsStartupTest>()))
            {
                await ValidateConfig(testServer);
                await ApiKeyAuthTest(testServer);
                
            }
        }

    }
}
