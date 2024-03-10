using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

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

        [Fact]
        public void ArgumentExceptions()
        {
            var services = new ServiceCollection();
            Action<IMvcBuilder>? nullAction = null;
            Assembly? nullAssembly = null;
            List<Assembly>? nullAssemblies = null;
            List<Assembly> baseAssemblies = new List<Assembly> { this.GetType().Assembly };
            OltInjectionAssemblyFilter? nullFilter = null;

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, nullFilter!, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, new OltInjectionAssemblyFilter(), nullAction));


            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, this.GetType().Assembly, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, this.GetType().Assembly, nullFilter!, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, this.GetType().Assembly, new OltInjectionAssemblyFilter(), nullAction));

            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssembly!, nullAction));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssembly!, nullFilter!, nullAction));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssembly!, new OltInjectionAssemblyFilter(), nullAction));


            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, baseAssemblies, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, baseAssemblies, nullFilter!, nullAction));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null!, baseAssemblies, new OltInjectionAssemblyFilter(), nullAction));

            try
            {
                OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssemblies!, nullAction);
                Assert.True(true);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, baseAssemblies, action => action.AddControllersAsServices());
                Assert.True(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}