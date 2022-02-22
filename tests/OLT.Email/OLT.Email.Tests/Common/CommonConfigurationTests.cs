using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Common
{

    public class CommonConfigurationTests
    {
        public class AppSettingsJsonDto
        {
            public OltEmailConfiguration EmailConfig { get; set; } = new OltEmailConfiguration();
        }


        private readonly OltEmailConfiguration _emailConfiguration;

        public CommonConfigurationTests(
            IOptions<OltEmailConfiguration> options)
        {
            _emailConfiguration = options.Value;
        }

        [Fact]
        public void EmailConfiguration()
        {
            var fromEmail = Faker.Internet.Email();
            var fromName = Faker.Name.FullName();
            
            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();

            var config = new OltEmailConfiguration();
            config.TestWhitelist.Domain.Add(whiteDomain);
            config.TestWhitelist.Email.Add(whiteEmail);
            config.From.Name = fromName;
            config.From.Email = fromEmail;

            Assert.False(config.Production);
            Assert.Equal(fromName, config.From.Name);
            Assert.Equal(fromEmail, config.From.Email);
            Assert.NotEmpty(config.TestWhitelist.Email);
            Assert.NotEmpty(config.TestWhitelist.Domain);
            Assert.Equal(whiteEmail, config.TestWhitelist.Email[0]);
            Assert.Equal(whiteDomain, config.TestWhitelist.Domain[0]);
            config.Production = true;
            Assert.True(config.Production);
        }

        [Fact]
        public async Task Options()
        {
            Assert.NotNull(_emailConfiguration);

            string fileName = "appsettings.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (FileStream openStream = File.OpenRead(filePath))
            {
                AppSettingsJsonDto? expectedConfig = await JsonSerializer.DeserializeAsync<AppSettingsJsonDto>(openStream);
                Assert.NotNull(expectedConfig);
                _emailConfiguration.Should().BeEquivalentTo(expectedConfig?.EmailConfig);
            }           

        }

       



    }
}