using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using OLT.Core;
using System;
using Xunit;

namespace OLT.Extensions.Configuration.Tests
{
    public class AzureAppConfigTests
    {
        private static string RunEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        [Fact]
        public void ExceptionTest()
        {
            var options = new AzureAppConfigurationOptions();
            Assert.Throws<ArgumentNullException>("options", () => OltAzureAppConfigExtensions.Connect(null, null));
            Assert.Throws<ArgumentNullException>("endpoint", () => OltAzureAppConfigExtensions.Connect(options, null));
            Assert.Throws<ArgumentNullException>("options", () => OltAzureAppConfigExtensions.OltAzureConfigDefault(null, null));
            Assert.Throws<ArgumentNullException>("config", () => OltAzureAppConfigExtensions.OltAzureConfigDefault(options, null));
        }


        [Fact]
        public void OptionsTests()
        {

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<AzureAppConfigTests>();

            var configuration = builder.Build();

            var endpoint = configuration.GetValue<string>("AZURE_APP_CONFIG_ENDPOINT") ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_ENDPOINT");


            var options = new AzureAppConfigurationOptions();

            try
            {
                var result = OltAzureAppConfigExtensions.Connect(options, endpoint);
                Assert.NotNull(result);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                var result = OltAzureAppConfigExtensions.Connect(options, endpoint, new EnvironmentCredential());
                Assert.NotNull(result);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                var result = OltAzureAppConfigExtensions.OltAzureConfigDefault(options, new OltOptionsAzureConfig("Nuget:", RunEnvironment));
                Assert.NotNull(result);
            }
            catch (Exception)
            {
                throw;
            }



        }


        [Fact]
        public void ConnectionTests()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<AzureAppConfigTests>();

            var configuration = builder.Build();

            try
            {
                var connectionString = configuration.GetValue<string>("AZURE_APP_CONFIG_CONNECTION_STRING") ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_CONNECTION_STRING");

                builder.AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString);
                    OltAzureAppConfigExtensions.OltAzureConfigDefault(options, new OltOptionsAzureConfig("Nuget:", "Development"));
                });

                var azureConfig = builder.Build();
                Assert.NotNull(azureConfig);
                var value = azureConfig.GetValue<string>("Value");
                Assert.Equal("ABC1234-DEFAULT", value);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                var connectionString = configuration.GetValue<string>("AZURE_APP_CONFIG_CONNECTION_STRING") ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_CONNECTION_STRING");

                builder.AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString);
                    OltAzureAppConfigExtensions.OltAzureConfigDefault(options, new OltOptionsAzureConfig("Nuget:", RunEnvironment));
                });

                var azureConfig = builder.Build();
                Assert.NotNull(azureConfig);
                var value = azureConfig.GetValue<string>("Value");
                Assert.Equal($"ABC1234-{RunEnvironment ?? "DEFAULT"}", value);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
