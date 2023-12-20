using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets;
using OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets.Startups;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests
{
    public class ControllerTests
    {

        private async Task<JwtBearerOptions> AuthTest(TestServer testServer)
        {
            var services = testServer.Host.Services;
            var schemeProvider = services.GetRequiredService<IAuthenticationSchemeProvider>();
            Assert.NotNull(schemeProvider);

            var optionsSnapshot = services.GetService<IOptionsSnapshot<JwtBearerOptions>>();
            Assert.NotNull(optionsSnapshot);

            var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
            Assert.NotNull(scheme);

            var schemeOptions = optionsSnapshot.Get(scheme.Name);
            Assert.NotNull(schemeOptions);

            var options = JwtTokenTestExts.GetOptions();
            
            Assert.Equal(options.RequireHttpsMetadata, schemeOptions.RequireHttpsMetadata);
            Assert.Equal(options.ValidateIssuer, schemeOptions.TokenValidationParameters.ValidateIssuer);
            Assert.Equal(options.ValidateAudience, schemeOptions.TokenValidationParameters.ValidateAudience);
            Assert.Equal(typeof(JwtBearerHandler), scheme.HandlerType);

            var response = await testServer.CreateRequest("/api").SendAsync("GET");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            return schemeOptions;
        }

        [Fact]
        public async Task ServerWithOptions()
        {
            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<WithOptionsStartupTest>()))
            {
                var schemeOptions = await AuthTest(testServer);
                Assert.Equal(JwtTokenTestExts.Authority, schemeOptions.Authority);
                Assert.Equal(JwtTokenTestExts.Audience, schemeOptions.Audience);                
            }
        }

        [Fact]
        public async Task ServerWithoutOptions()
        {
            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<WithoutOptionsStartupTest>()))
            {
                var schemeOptions = await AuthTest(testServer);
                Assert.NotEqual(JwtTokenTestExts.Authority, schemeOptions.Authority);
                Assert.NotEqual(JwtTokenTestExts.Audience, schemeOptions.Audience);
            }
        }

    }
}
