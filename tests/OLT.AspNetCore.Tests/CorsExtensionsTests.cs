#nullable disable
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OLT.Constants;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class CorsExtensionsTests
    {
        [Fact]
        public void UseCorsTests()
        {
            List<Assembly> baseAssemblies = new List<Assembly> { this.GetType().Assembly };

            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());

            OltCorsExtensions.AddCors(services, baseAssemblies);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<CorsOptions>>();
                var expectedPolicy = options.Value.GetPolicy(OltAspNetDefaults.CorsPolicies.Wildcard);
                Assert.True(expectedPolicy.AllowAnyOrigin);
                Assert.True(expectedPolicy.AllowAnyMethod);
                Assert.True(expectedPolicy.AllowAnyHeader);
                Assert.False(expectedPolicy.SupportsCredentials);
            }

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<CorsOptions>>();
                var expectedPolicy = options.Value.GetPolicy(OltAspNetDefaults.CorsPolicies.Disabled);
                Assert.False(expectedPolicy.AllowAnyOrigin);
                Assert.False(expectedPolicy.AllowAnyMethod);
                Assert.False(expectedPolicy.AllowAnyHeader);
                Assert.False(expectedPolicy.SupportsCredentials);
            }

            var hostingOptions = new OltAspNetHostingOptions
            {
                CorsPolicyName = string.Empty
            };

            var result = OltCorsExtensions.UseCors(new ApplicationBuilder(services.BuildServiceProvider()), hostingOptions);
            Assert.NotNull(result);

            result = OltCorsExtensions.UseCors(new ApplicationBuilder(services.BuildServiceProvider()), new OltAspNetHostingOptions {  CorsPolicyName = OltAspNetDefaults.CorsPolicies.Wildcard });
            Assert.NotNull(result);

            result = OltCorsExtensions.UseCors(new ApplicationBuilder(services.BuildServiceProvider()), new OltAspNetHostingOptions { CorsPolicyName = OltAspNetDefaults.CorsPolicies.Disabled });
            Assert.NotNull(result);

        }

        [Fact]
        public void ArgumentExceptions()
        {
            var services = new ServiceCollection();
            IOltAspNetCoreCorsPolicy nullPolicy = null;
            List<Assembly> nullAssemblies = null;
            List<Assembly> baseAssemblies = new List<Assembly> { this.GetType().Assembly };

            Assert.Throws<ArgumentNullException>("services", () => OltCorsExtensions.AddCors(null!, baseAssemblies));
            Assert.Throws<ArgumentNullException>("assembliesToScan", () => OltCorsExtensions.AddCors(services, nullAssemblies!));

            Assert.Throws<ArgumentNullException>("services", () => OltCorsExtensions.AddCors(null!, nullPolicy!));
            Assert.Throws<ArgumentNullException>("policy", () => OltCorsExtensions.AddCors(services, nullPolicy!));

            var app = new ApplicationBuilder(services.BuildServiceProvider());
            OltAspNetHostingOptions nullOptions = null;


            Assert.Throws<ArgumentNullException>("app", () => OltCorsExtensions.UseCors(null!, nullOptions));
            Assert.Throws<ArgumentNullException>("options", () => OltCorsExtensions.UseCors(app, nullOptions));

        }
    }
}