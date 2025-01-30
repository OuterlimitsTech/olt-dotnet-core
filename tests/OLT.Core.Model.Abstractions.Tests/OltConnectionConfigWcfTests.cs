namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigWcfTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsPropertiesCorrectly()
    {
        // Arrange
        var config = new OltConnectionConfigWcf();
        string connectionString = "endpoint=https://domain.com/wcf-service/service.svc;username=UsernameHere;password=PasswordHere";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("https://domain.com/wcf-service/service.svc", config.Endpoint);
        Assert.Equal("UsernameHere", config.Username);
        Assert.Equal("PasswordHere", config.Password);
    }

    [Fact]
    public void Parse_EmptyConnectionString_SetsPropertiesToNull()
    {
        // Arrange
        var config = new OltConnectionConfigWcf();
        string connectionString = "";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Endpoint);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
    }

    [Fact]
    public void Parse_NullConnectionString_SetsPropertiesToNull()
    {
        // Arrange
        var config = new OltConnectionConfigWcf();
        string? connectionString = null;

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Endpoint);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
    }


    [Theory]
    [InlineData("endpoint=https://api.domain.com/service.svc;username=guest;password=abc123", "https://api.domain.com/service.svc", "guest", "abc123")]
    [InlineData("endpoint=https://api.domain.com/service.svc;username=guest;password=", "https://api.domain.com/service.svc", "guest", null)]
    [InlineData("endpoint=https://api.domain.com/service.svc;username=guest", "https://api.domain.com/service.svc", "guest", null)]
    [InlineData("endpoint=https://api.domain.com/service.svc;password=abc123", "https://api.domain.com/service.svc", null, "abc123")]
    [InlineData("endpoint=https://api.domain.com/service.svc;username=;password=abc123", "https://api.domain.com/service.svc", null, "abc123")]
    [InlineData("username=guest;password=abc123", null, "guest", "abc123")]
    [InlineData("host=;username=guest;password=abc123", null, "guest", "abc123")]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
    [InlineData("", null, null, null)]
    [InlineData(" ", null, null, null)]
    [InlineData(null, null, null, null)]
    public void Parse_ConnectionStrings(string? connectionString, string? expectedEndpoint, string? expectedUsername, string? expectedPassword)
    {
        var config = new OltConnectionConfigWcf();
        config.Parse(connectionString);

        Assert.Equal(expectedEndpoint, config.Endpoint);
        Assert.Equal(expectedUsername, config.Username);
        Assert.Equal(expectedPassword, config.Password);
    }
}
