using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Extensions.DependencyInjection.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace OLT.Extensions.DependencyInjection.Tests
{
    public class UnitTests
    {
        [Fact]
        public void ExceptionTest()
        {
            var services = new ServiceCollection();
            Assembly? nullRef = null;
            List<Assembly>? nullList = null;

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionExtensions.AddOltInjection(null!, null));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionExtensions.AddOltInjection(null!, nullRef!, null));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionExtensions.AddOltInjection(null!, nullList!, null));
            Assert.Throws<ArgumentNullException>("baseAssembly", () => OltServiceCollectionExtensions.AddOltInjection(services, nullRef!, null));

            try
            {
                OltServiceCollectionExtensions.AddOltInjection(services, nullList!, null);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void InjectionTest()
        {
            var services = new ServiceCollection();
            OltServiceCollectionExtensions.AddOltInjection(services, null)
                .AddScoped<IOltIdentity, TestIdentity>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>())
                ;
            
            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltDbAuditUser>());
            }           
        }

        [Fact]
        public void InjectionAssemblyTest()
        {
            var services = new ServiceCollection();
            OltServiceCollectionExtensions.AddOltInjection(services, this.GetType().Assembly)
                .AddScoped<IOltIdentity, TestIdentity>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltDbAuditUser>());
            }
        }

        [Fact]
        public void InjectionAssemblyListTest()
        {
            var services = new ServiceCollection();
            OltServiceCollectionExtensions.AddOltInjection(services, new List<Assembly>() { this.GetType().Assembly })
                .AddScoped<IOltIdentity, TestIdentity>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltDbAuditUser>());
            }
        }

        [Fact]
        public void Test_Scan()
        {
            var services = new ServiceCollection();
            OltServiceCollectionExtensions
                .Scan(services, opt =>
                {
                    opt.IncludeAssembly(this.GetType().Assembly);
                    opt.DeepScan();
                })
                .AddScoped<IOltIdentity, TestIdentity>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltDbAuditUser>());
            }
        }

    }
}