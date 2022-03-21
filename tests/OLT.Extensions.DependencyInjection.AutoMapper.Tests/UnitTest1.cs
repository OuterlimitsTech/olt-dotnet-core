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
        }
    }
}