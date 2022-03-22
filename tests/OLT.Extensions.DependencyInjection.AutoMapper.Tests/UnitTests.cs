using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace OLT.Extensions.DependencyInjection.AutoMapper.Tests
{
    public class UnitTests
    {
        [Fact]
        public void ExceptionTest()
        {
            var services = new ServiceCollection();
            Assembly nullRef = null;
            List<Assembly> nullList = null;
            Action<IMapperConfigurationExpression> configAction = null;

            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(null));
            Assert.Throws<ArgumentNullException>("includeAssemblyScan", () => OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(null, nullRef));
            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(null, nullList));

            Assert.Throws<ArgumentNullException>("includeAssemblyScan", () => OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, nullRef));

            try
            {
                OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, nullList);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

            try
            {
                OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, new List<Assembly>() { this.GetType().Assembly }, configAction);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

            try
            {
                OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, new List<Assembly>() { this.GetType().Assembly }, cfg => cfg.DisableConstructorMapping(), ServiceLifetime.Scoped);
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
            OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services);

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltAdapterResolver>());
            }
        }

        [Fact]
        public void InjectionAssemblyTest()
        {
            var services = new ServiceCollection();
            OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, this.GetType().Assembly);

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltAdapterResolver>());
            }
        }

        [Fact]
        public void InjectionAssemblyListTest()
        {
            var services = new ServiceCollection();
            OltServiceCollectionAutoMapperExtensions.AddOltInjectionAutoMapper(services, new List<Assembly>() { this.GetType().Assembly }, cfg => cfg.DisableConstructorMapping(), ServiceLifetime.Scoped);

            using (var provider = services.BuildServiceProvider())
            {
                Assert.NotNull(provider.GetService<IOltAdapterResolver>());
            }
        }
    }
}