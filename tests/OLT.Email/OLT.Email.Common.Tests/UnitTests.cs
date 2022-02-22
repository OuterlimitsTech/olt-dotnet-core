using FluentAssertions;
using Microsoft.Extensions.Options;
using OLT.Email.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Common.Tests
{

    public class ConfigurationTests
    {
        public class AppSettingsJsonDto
        {
            public OltEmailConfiguration EmailConfig { get; set; } = new OltEmailConfiguration();
        }


        private readonly OltEmailConfiguration _emailConfiguration;

        public ConfigurationTests(
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
            var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";

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

            
            //Assert.False(config.SendEmail(Faker.Internet.FreeEmail()));
            //Assert.True(config.SendEmail(whiteEmail));
            //Assert.True(config.SendEmail(whiteDomainEmail));

            config.Production = true;
            Assert.True(config.Production);

            //Assert.True(config.SendEmail(Faker.Internet.FreeEmail()));
            //Assert.True(config.SendEmail(whiteEmail));
            //Assert.True(config.SendEmail(whiteDomainEmail));

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