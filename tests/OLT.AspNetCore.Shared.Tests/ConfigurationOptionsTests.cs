using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using OLT.Core;
using System;
using System.IO;
using Xunit;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Shared.Tests
{
    /// <summary>
    /// This test has to be independent otherwise it gets an injection error
    /// </summary>
    public class ConfigurationOptionsTests
    {


        private readonly OltAspNetAppSettings _appSettings;

        public ConfigurationOptionsTests(IOptions<OltAspNetAppSettings> options)
        {
            _appSettings = options.Value;
        }


        [Fact]
        public async Task Options()
        {
            Assert.NotNull(_appSettings);

            string fileName = "appsettings.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (FileStream openStream = File.OpenRead(filePath))
            {
                AppSettingsJsonDto expectedConfig = await JsonSerializer.DeserializeAsync<AppSettingsJsonDto>(openStream);
                Assert.NotNull(expectedConfig);
                _appSettings.Should().BeEquivalentTo(expectedConfig?.AppSettings);
            }

        }


        public class AppSettingsJsonDto
        {
            public OltAspNetAppSettings AppSettings { get; set; } = new OltAspNetAppSettings();
        }
    }
}