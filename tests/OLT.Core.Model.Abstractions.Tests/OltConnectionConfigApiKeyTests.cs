namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigApiKeyTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsEndpointAndApiKey()
    {
        // Arrange
        var connectionString = "endpoint=https://api.domain.com;apikey=API_KEY_HERE";
        var config = new OltConnectionConfigApiKey();

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("https://api.domain.com/", config.Endpoint);
        Assert.Equal("API_KEY_HERE", config.ApiKey);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutEndpoint_SetsEndpointToNull()
    {
        // Arrange
        var connectionString = "apikey=API_KEY_HERE";
        var config = new OltConnectionConfigApiKey();

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Endpoint);
        Assert.Equal("API_KEY_HERE", config.ApiKey);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutApiKey_SetsApiKeyToNull()
    {
        // Arrange
        var connectionString = "endpoint=https://api.domain.com";
        var config = new OltConnectionConfigApiKey();

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("https://api.domain.com/", config.Endpoint);
        Assert.Null(config.ApiKey);
    }

    [Fact]
    public void Parse_EndpointWithoutTrailingSlash_AddsTrailingSlash()
    {
        // Arrange
        var connectionString = "endpoint=https://api.domain.com;apikey=API_KEY_HERE";
        var config = new OltConnectionConfigApiKey();

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("https://api.domain.com/", config.Endpoint);
    }

    [Theory]
    [InlineData("endpoint=https://api.domain.com;apikey=abc123", "https://api.domain.com/", "abc123")]
    [InlineData("endpoint=https://api.domain.com/;apikey=abc123", "https://api.domain.com/", "abc123")]
    [InlineData("endpoint=https://api.domain.com;apikey=", "https://api.domain.com/", null)]
    [InlineData("endpoint=https://api.domain.com", "https://api.domain.com/", null)]
    [InlineData("endpoint=https://api.domain.com/", "https://api.domain.com/", null)]
    [InlineData("endpoint=;apikey=abc123", null, "abc123")]
    [InlineData("apikey=abc123", null, "abc123")]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null)]
    [InlineData("", null, null)]
    [InlineData(" ", null, null)]
    [InlineData(null, null, null)]
    public void Parse_ConnectionStrings(string connectionString, string expectedEndpoint, string expectedApiKey)
    {
        var config = new OltConnectionConfigApiKey();
        config.Parse(connectionString);

        Assert.Equal(expectedEndpoint, config.Endpoint);
        Assert.Equal(expectedApiKey, config.ApiKey);
    }
}
