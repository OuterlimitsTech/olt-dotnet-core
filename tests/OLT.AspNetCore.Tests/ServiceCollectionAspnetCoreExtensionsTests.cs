using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Tests.Assets;
using OLT.Constants;
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
        private static string RunEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

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
                Assert.Equal(RunEnvironment, hostService.EnvironmentName);                
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
                var dbAuditUser = testServer.Services.GetService<IOltDbAuditUser>();
                Assert.NotNull(dbAuditUser);
                Assert.Null(dbAuditUser.GetDbUsername());                
            }
        }

        [Fact]
        public void ArgumentExceptions()
        {
            var services = new ServiceCollection();
            Action<IMvcBuilder> nullAction = null;
            Assembly nullAssembly = null;
            List<Assembly> nullAssemblies = null;
            List<Assembly> baseAssemblies = new List<Assembly> { this.GetType().Assembly };

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, nullAction));
            

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, this.GetType().Assembly, nullAction));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssembly, nullAction));


            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(null, baseAssemblies, nullAction));
            
            try
            {
                OltServiceCollectionAspnetCoreExtensions.AddOltAspNetCore(services, nullAssemblies, nullAction);
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

        [Fact]
        public void OptionsApiVersionTests()
        { 

            Assert.Equal("api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Query);
            Assert.Equal("v", OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType);
            Assert.Equal("x-api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Header);


            var model = new OltOptionsApiVersion();
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Query, model.Parameter.Query);
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType, model.Parameter.MediaType);
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Header, model.Parameter.Header);            ;
            Assert.True(model.AssumeDefaultVersion);
            model.DefaultVersion.Should().BeEquivalentTo(ApiVersion.Default);

            model.AssumeDefaultVersion = false;

            var version = Faker.Internet.UserName();
            model.Parameter.Query = version;
            model.Parameter.MediaType = version;
            model.Parameter.Header = version;

            Assert.Equal(version, model.Parameter.Query);
            Assert.Equal(version, model.Parameter.MediaType);
            Assert.Equal(version, model.Parameter.Header);
            Assert.False(model.AssumeDefaultVersion);



        }
    }
}