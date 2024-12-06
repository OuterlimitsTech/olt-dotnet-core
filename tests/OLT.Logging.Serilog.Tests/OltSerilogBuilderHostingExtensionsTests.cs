//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using OLT.Core;
//using Serilog;

//namespace OLT.Logging.Serilog.Tests;

//public class OltSerilogBuilderHostingExtensionsTests
//{
//    [Fact]
//    public void ConfigureLogging_ConfiguresSerilogCorrectly()
//    {
//        // Arrange
//        var loggerConfiguration = new LoggerConfiguration();
//        var configurationMock = new Mock<IConfigurationManager>();
//        var services = new ServiceCollection();
//        var builderMock = new Mock<IOltSerilogHostApplicationBuilder>();

//        builderMock.SetupGet(b => b.LoggerConfiguration).Returns(loggerConfiguration);
//        builderMock.SetupGet(b => b.Configuration).Returns(configurationMock.Object);
//        builderMock.SetupGet(b => b.Services).Returns(services);

//        // Act
//        var result = builderMock.Object.ConfigureLogging();

//        // Assert
//        Assert.NotNull(Log.Logger);
//        builderMock.Verify(b => b.AddSerilog(), Times.Once);
//    }

//    [Fact]
//    public void AddSerilog_AddsSerilogToServices()
//    {
//        // Arrange
//        var services = new ServiceCollection();
//        var builderMock = new Mock<IOltSerilogHostApplicationBuilder>();

//        builderMock.SetupGet(b => b.Services).Returns(services);

//        // Act
//        var result = builderMock.Object.AddSerilog();

//        // Assert
//        var serviceProvider = services.BuildServiceProvider();
//        var logger = serviceProvider.GetService<ILogger>();
//        Assert.NotNull(logger);
//    }

//    [Fact]
//    public void BuildSerilogConfig_ConfiguresLoggerConfigurationCorrectly()
//    {
//        // Arrange
//        var loggerConfiguration = new LoggerConfiguration();
//        var configurationMock = new Mock<IConfiguration>();

//        // Act
//        var result = loggerConfiguration.BuildSerilogConfig(configurationMock.Object);

//        // Assert
//        Assert.NotNull(result);
//    }
//}


