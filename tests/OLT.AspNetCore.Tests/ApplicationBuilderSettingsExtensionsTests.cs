using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class ApplicationBuilderSettingsExtensionsTests
    {
        public static IEnumerable<object[]> OptionsData =>
            new List<object[]>
            {
                        new object[] { new OltAspNetHostingOptions { PathBase = "/test", UseHsts = true, DisableHttpsRedirect = false, ShowExceptionDetails = true } },
                        new object[] { new OltAspNetHostingOptions { PathBase = "", UseHsts = true, DisableHttpsRedirect = false, ShowExceptionDetails = false } },
                        new object[] { new OltAspNetHostingOptions { PathBase = null, UseHsts = false, DisableHttpsRedirect = true, ShowExceptionDetails = false } },
            };

        [Theory]
        [MemberData(nameof(OptionsData))]
        public async Task OptionsTest(OltAspNetHostingOptions options)
        {
            var response = await RunTest(options);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<HttpResponseMessage> RunTest(OltAspNetHostingOptions options)
        {

            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                    })
                    .Configure(app =>
                    {
                        OltApplicationBuilderSettingsExtensions.UsePathBase(app, options);
                        OltApplicationBuilderSettingsExtensions.UseDeveloperExceptionPage(app, options);
                        OltApplicationBuilderSettingsExtensions.UseHsts(app, options);
                        OltApplicationBuilderSettingsExtensions.UseHttpsRedirection(app, options);
                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                        var xyz = app.Build();
                        var test = 1234;
                    });
                })
                .StartAsync();

            return await host.GetTestClient().GetAsync("/api");

        }
    }
}
