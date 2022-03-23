using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{

    public class ServiceCollectionAspnetCoreExtensionsTests
    {

        [Fact]
        public void HostServiceTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var hostService = testServer.Services.GetService<IOltHostService>();
                Assert.NotNull(hostService);
                var result = hostService.ResolveRelativePath("~/test");
                Assert.Equal(Path.Combine(AppContext.BaseDirectory, "test"), result);
                Assert.Equal("OLT.AspNetCore.Tests", hostService.ApplicationName);
                Assert.Equal("Production", hostService.EnvironmentName);                
            }
        }

        [Fact]
        public void IdentityAnonymousTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {                
                var identity = testServer.Services.GetService<IOltIdentity>();
                Assert.NotNull(identity);
                Assert.True(identity.IsAnonymous);
            }
        }

        [Fact]
        public void ArgumentExceptions()
        {
            var services = new ServiceCollection();
            OltAspNetAppSettings nullSettings = null;
            Action<IMvcBuilder> nullAction = null;
            Assembly nullAssembly = null;
            List<Assembly> nullAssemblies = null;
            List<Assembly> baseAssemblies = new List<Assembly> { this.GetType().Assembly };

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, new OltAspNetAppSettings(), nullAction));
            Assert.Throws<ArgumentNullException>("settings", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullSettings, nullAction));


            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, new OltAspNetAppSettings(), this.GetType().Assembly, nullAction));
            Assert.Throws<ArgumentNullException>("settings", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullSettings, this.GetType().Assembly, nullAction));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, new OltAspNetAppSettings(), nullAssembly, nullAction));


            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, new OltAspNetAppSettings(), baseAssemblies, nullAction));
            Assert.Throws<ArgumentNullException>("settings", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullSettings, baseAssemblies, nullAction));
            
            try
            {
                OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, new OltAspNetAppSettings(), nullAssemblies, nullAction);
                Assert.True(true);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, new OltAspNetAppSettings(), baseAssemblies, action => action.AddControllersAsServices());
                Assert.True(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}