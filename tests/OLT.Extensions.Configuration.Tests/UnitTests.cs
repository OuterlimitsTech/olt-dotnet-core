using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using OLT.Core;
using System;

namespace OLT.Extensions.Configuration.Tests
{
    public class UnitTests
    {
        readonly string name = "DBConnection";

        public UnitTests()
        {
            Environment.SetEnvironmentVariable($"ConnectionStrings__{name}", "Value3");
        }

        [Fact]
        public void ExceptionTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            Assert.Throws<ArgumentNullException>("config", () => OltConfigurationExtensions.GetOltConnectionString(null, null));
            Assert.Throws<ArgumentNullException>("config", () => OltConfigurationExtensions.GetOltConnectionString(null, name));
            Assert.Throws<ArgumentNullException>("name", () => OltConfigurationExtensions.GetOltConnectionString(configuration, null));
        }

        [Fact]
        public void OverrideTest()
        {
            

            var myConfiguration = new Dictionary<string, string>
            {
                {$"connection-strings:{name}", "Value1"},
                {$"ConnectionStrings:{name}", "Value2"}
            };

           

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            Assert.Equal("Value1", OltConfigurationExtensions.GetOltConnectionString(configuration, name));
        }

    
        [Fact]
        public void StandardTest()
        {

            var myConfiguration = new Dictionary<string, string>
            {
                {$"ConnectionStrings:{name}", "Value2"}
            };

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            Assert.Equal("Value2", OltConfigurationExtensions.GetOltConnectionString(configuration, name));
        }

        [Fact]
        public void EnvironmentTest()
        {

            var myConfiguration = new Dictionary<string, string>
            {
                
            };


            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            Assert.Equal("Value3", OltConfigurationExtensions.GetOltConnectionString(configuration, name));

        }
    }
}