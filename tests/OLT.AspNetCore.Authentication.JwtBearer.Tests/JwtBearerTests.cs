using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using Xunit;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests
{
    public class JwtBearerTests
    {
        public const string Secret = "TopSecretValue";

        [Fact]
        public void ArgumentExceptions()
        {
            var services = new ServiceCollection();
            var invalidOptions = new OltAuthenticationJwtBearer(null);
            var validOptions = new OltAuthenticationJwtBearer(Secret);
            var builder = new AuthenticationBuilder(services);

            Action<JwtBearerOptions> action = (JwtBearerOptions opts) =>
            {
                opts.RequireHttpsMetadata = false;
                opts.Authority = "local";
                opts.Audience = "local";
            };

            Action<AuthenticationOptions> authAction = (AuthenticationOptions opts) =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            };

            Assert.Throws<ArgumentNullException>("services", () => OltAuthenticationJwtExtensions.AddJwtBearer(null, validOptions));
            Assert.Throws<ArgumentNullException>("schemeBuilder", () => OltAuthenticationJwtExtensions.AddJwtBearer<OltAuthenticationJwtBearer>(services, null));


            Assert.Throws<ArgumentNullException>("services", () => OltAuthenticationJwtExtensions.AddJwtBearer(null, validOptions, null));
            Assert.Throws<ArgumentNullException>("schemeBuilder", () => OltAuthenticationJwtExtensions.AddJwtBearer<OltAuthenticationJwtBearer>(services, null, action));

            Assert.Throws<ArgumentNullException>("services", () => OltAuthenticationJwtExtensions.AddJwtBearer(null, validOptions, action, authAction));
            Assert.Throws<ArgumentNullException>("schemeBuilder", () => OltAuthenticationJwtExtensions.AddJwtBearer<OltAuthenticationJwtBearer>(services, null, action, authAction));

            Assert.Throws<ArgumentNullException>("builder", () => invalidOptions.AddScheme(null, null));
            Assert.Throws<ArgumentNullException>("builder", () => new OltAuthenticationJwtBearer(null).AddScheme(null, null));

            Assert.Throws<ArgumentNullException>("JwtSecret", () => new OltAuthenticationJwtBearer(null).AddScheme(builder, null));
            Assert.Throws<ArgumentNullException>("JwtSecret", () => new OltAuthenticationJwtBearer(null).AddScheme(builder, action));            


        }



        [Fact]
        public void SchemeBuilder()
        {
            var token = Guid.NewGuid().ToString();
            var services = new ServiceCollection();
            var builder = new AuthenticationBuilder(services);

            var model = new OltAuthenticationJwtBearer(null);            
            Assert.Null(model.JwtSecret);
            Assert.True(model.RequireHttpsMetadata);
            Assert.False(model.ValidateIssuer);
            Assert.False(model.ValidateAudience);

            model = new OltAuthenticationJwtBearer(token);
            model.RequireHttpsMetadata = false;
            model.ValidateIssuer = true;
            model.ValidateAudience = true;
            
            Assert.Equal(token, model.JwtSecret);
            Assert.False(model.RequireHttpsMetadata);
            Assert.True(model.ValidateIssuer);
            Assert.True(model.ValidateAudience);



            try
            {
                model.AddScheme(builder, null);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
                        
            Action<JwtBearerOptions> action = (JwtBearerOptions opts) =>
            {
#pragma warning disable S1481 // Unused local variables should be removed
                var opt2 = opts;
#pragma warning restore S1481 // Unused local variables should be removed
                Assert.True(true);
            };

            try
            {
                model.AddScheme(builder, action);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

        }
    }
}