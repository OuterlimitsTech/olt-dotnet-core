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
