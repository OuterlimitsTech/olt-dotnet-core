using OLT.Constants;
using OLT.Core;
using Xunit;

namespace OLT.AspNetCore.Shared.Tests
{
    public class ModelTests
    {

        [Fact]
        public void AspNetAppSettingTests()
        {

            Assert.Equal("Olt_CorsPolicy_Disabled", OltAspNetDefaults.CorsPolicyName);
            Assert.Equal("Olt_CorsPolicy_Disabled", OltAspNetDefaults.CorsPolicies.Disabled);
            Assert.Equal("Olt_CorsPolicy_Wildcard", OltAspNetDefaults.CorsPolicies.Wildcard);

            Assert.Equal(OltAspNetDefaults.CorsPolicies.Disabled, OltAspNetDefaults.CorsPolicyName);

            var model = new OltAspNetAppSettings();
            Assert.NotNull(model as IOltOptionsAspNet);
            Assert.NotNull(model.Hosting);
            Assert.NotNull(model.Hosting as IOltOptionsAspNetHosting);
            Assert.Equal(OltAspNetDefaults.CorsPolicyName, model.Hosting.CorsPolicyName);
            Assert.Null(model.Hosting.PathBase);
            Assert.False(model.Hosting.UseHsts);
            Assert.False(model.Hosting.DisableHttpsRedirect);
            Assert.False(model.Hosting.ShowExceptionDetails);


            var pathBase = Faker.Internet.DomainName();
            var policy = Faker.Internet.UserName();

            model.Hosting.PathBase = pathBase;
            model.Hosting.CorsPolicyName = policy;
            model.Hosting.UseHsts = true;
            model.Hosting.DisableHttpsRedirect = true;
            model.Hosting.ShowExceptionDetails = true;

            Assert.Equal(pathBase, model.Hosting.PathBase);
            Assert.Equal(policy, model.Hosting.CorsPolicyName);
            Assert.True(model.Hosting.UseHsts);
            Assert.True(model.Hosting.DisableHttpsRedirect);
            Assert.True(model.Hosting.ShowExceptionDetails);


        }

        [Fact]
        public void OptionsApiVersionTests()
        {

            Assert.Equal("api-version", OltAspNetDefaults.ApiQueryParameterName);

            Assert.Equal(OltAspNetDefaults.CorsPolicies.Disabled, OltAspNetDefaults.CorsPolicyName);

            var model = new OltOptionsApiVersion();
            Assert.NotNull(model as IOltOptionsApiVersion);
            Assert.Equal(OltAspNetDefaults.ApiQueryParameterName, model.ApiQueryParameterName);
            Assert.True(model.Enabled);
            Assert.True(model.AssumeDefaultVersionWhenUnspecified);

            model.AssumeDefaultVersionWhenUnspecified = false;
            model.Enabled = false;
            
            var version = Faker.Internet.UserName();
            model.ApiQueryParameterName = version;

            Assert.Equal(version, model.ApiQueryParameterName);
            Assert.False(model.Enabled);
            Assert.False(model.AssumeDefaultVersionWhenUnspecified);



        }
    }
}