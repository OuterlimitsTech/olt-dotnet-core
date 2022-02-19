using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class ConfigurationTests
    {
        public class AppSettingsJsonDto
        {
            public OltEmailConfigurationSendGrid SendGrid { get; set; } = new OltEmailConfigurationSendGrid();
        }


        private readonly OltEmailConfigurationSendGrid _emailConfiguration;

        public ConfigurationTests(IOptions<OltEmailConfigurationSendGrid> options)
        {
            _emailConfiguration = options.Value;
        }

        [Fact]
        public void EmailConfiguration()
        {
            var apiKey = Guid.NewGuid().ToString();
            var fromEmail = Faker.Internet.Email();
            var fromName = Faker.Name.FullName();

            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();
            var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";

            var config = new OltEmailConfigurationSendGrid();
            config.ApiKey = apiKey;
            config.TestWhitelist.Domain.Add(whiteDomain);
            config.TestWhitelist.Email.Add(whiteEmail);
            config.From.Name = fromName;
            config.From.Email = fromEmail;

            Assert.False(config.Production);
            Assert.Equal(apiKey, config.ApiKey);
            Assert.Equal(fromName, config.From.Name);
            Assert.Equal(fromEmail, config.From.Email);
            Assert.NotEmpty(config.TestWhitelist.Email);
            Assert.NotEmpty(config.TestWhitelist.Domain);
            Assert.Equal(whiteEmail, config.TestWhitelist.Email[0]);
            Assert.Equal(whiteDomain, config.TestWhitelist.Domain[0]);

            //Assert.False(config.AllowSend(Faker.Internet.FreeEmail()));
            //Assert.True(config.AllowSend(whiteEmail));
            //Assert.True(config.AllowSend(whiteDomainEmail));

            config.Production = true;
            Assert.True(config.Production);

            //Assert.True(config.AllowSend(Faker.Internet.FreeEmail()));
            //Assert.True(config.AllowSend(whiteEmail));
            //Assert.True(config.AllowSend(whiteDomainEmail));

        }

        [Fact]
        public async Task Options()
        {
            Assert.NotNull(_emailConfiguration);

            string fileName = "appsettings.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (FileStream openStream = File.OpenRead(filePath))
            {
                AppSettingsJsonDto expectedConfig = await JsonSerializer.DeserializeAsync<AppSettingsJsonDto>(openStream);
                Assert.NotNull(expectedConfig);
                _emailConfiguration.Should().BeEquivalentTo(expectedConfig?.SendGrid);
            }

        }

    }
}