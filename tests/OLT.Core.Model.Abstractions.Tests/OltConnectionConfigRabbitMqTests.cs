namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigRabbitMqTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsPropertiesCorrectly()
    {
        // Arrange
        var config = new OltConnectionConfigRabbitMq();
        string connectionString = "host=rabbitmq://localhost:5672;username=guest;password=guest";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("rabbitmq://localhost:5672", config.Host);
        Assert.Equal("guest", config.Username);
        Assert.Equal("guest", config.Password);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutHost_SetsHostToNull()
    {
        // Arrange
        var config = new OltConnectionConfigRabbitMq();
        string connectionString = "username=guest;password=guest";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Equal("guest", config.Username);
        Assert.Equal("guest", config.Password);
    }

    [Fact]
    public void Parse_EmptyConnectionString_SetsPropertiesToNull()
    {
        // Arrange
        var config = new OltConnectionConfigRabbitMq();
        string connectionString = "";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
    }

    [Theory]
    [InlineData("host=rabbitmq://localhost:5672;username=guest;password=abc123", "rabbitmq://localhost:5672", "guest", "abc123")]
    [InlineData("host=rabbitmq://localhost:5672;username=guest;password=", "rabbitmq://localhost:5672", "guest", null)]
    [InlineData("host=rabbitmq://localhost:5672;username=guest", "rabbitmq://localhost:5672", "guest", null)]
    [InlineData("host=rabbitmq://localhost:5672;password=abc123", "rabbitmq://localhost:5672", null, "abc123")]
    [InlineData("host=rabbitmq://localhost:5672;username=;password=abc123", "rabbitmq://localhost:5672", null, "abc123")]
    [InlineData("username=guest;password=abc123", null, "guest", "abc123")]
    [InlineData("host=;username=guest;password=abc123", null, "guest", "abc123")]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
    [InlineData("", null, null, null)]
    [InlineData(" ", null, null, null)]
    [InlineData(null, null, null, null)]
    public void Parse_ConnectionStrings(string connectionString, string expectedHost, string expectedUsername, string expectedPassword)
    {
        var config = new OltConnectionConfigRabbitMq();
        config.Parse(connectionString);

        Assert.Equal(expectedHost, config.Host);
        Assert.Equal(expectedUsername, config.Username);
        Assert.Equal(expectedPassword, config.Password);
    }
}
