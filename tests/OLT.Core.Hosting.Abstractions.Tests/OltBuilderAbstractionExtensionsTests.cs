using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace OLT.Core.Hosting.Abstractions.Tests;

public class OltBuilderAbstractionExtensionsTests
{ 
    [Fact]
    public void AddOltIdentity_ShouldRegisterServicesCorrectly()
    {
        // Arrange
        var builder = new TestApplicationBuilder(Host.CreateApplicationBuilder());
        

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var oltIdentity = serviceProvider.GetService<IOltIdentity>();
        var oltDbAuditUser = serviceProvider.GetService<IOltDbAuditUser>();
        var oltIdentityConcrete = serviceProvider.GetService<TestIdentity>();

        Assert.NotNull(oltIdentity);
        Assert.NotNull(oltDbAuditUser);
        Assert.NotNull(oltIdentityConcrete);
        Assert.IsType<TestIdentity>(oltIdentity);
        Assert.IsType<TestIdentity>(oltDbAuditUser);
    }

    [Fact]
    public void AddDevelopmentConfig_ShouldRegisterDevelopmentCorrectly()
    {
        // Arrange
        var builder = new TestApplicationBuilder(Host.CreateApplicationBuilder());
        builder.AddDevelopmentConfig<TestApplicationBuilder>(true);

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>(); // as IConfigurationSource;
        var root = (IConfigurationRoot)configuration;
        var providers = root?.Providers?.Select(s => s.ToString()).ToList() ?? new List<string?>();

        Assert.Contains("appsettings.Development.json", providers[^2]);
        Assert.Contains("secrets.json", providers[^1]);
    }

    [Fact]
    public void AddDevelopmentConfig_MissingRegisterDevelopmentCorrectly()
    {
        // Arrange
        var builder = new TestApplicationBuilder(Host.CreateApplicationBuilder());
        builder.AddDevelopmentConfig<TestApplicationBuilder>(false);

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>(); // as IConfigurationSource;
        var root = (IConfigurationRoot)configuration;
        var providers = root?.Providers?.Select(s => s.ToString()).ToList() ?? new List<string?>();

        Assert.DoesNotContain("appsettings.Development.json", providers[^2]);
        Assert.DoesNotContain("secrets.json", providers[^1]);
    }


    public class TestApplicationBuilder : OltHostApplicationBuilder<HostApplicationBuilder>
    {
        public TestApplicationBuilder([NotNull] HostApplicationBuilder builder) : base(builder)
        {
        }

        public override void AddConfiguration()
        {

        }

        public override void AddLoggingConfiguration()
        {

        }

        public override void AddServices()
        {
            this.AddOltIdentity<IOltIdentity, TestIdentity>();            
        }
    }


}
