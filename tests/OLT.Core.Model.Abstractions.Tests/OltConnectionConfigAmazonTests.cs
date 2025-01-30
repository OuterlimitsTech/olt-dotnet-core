namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigAmazonTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsPropertiesCorrectly()
    {
        // Arrange
        var config = new OltConnectionConfigAmazon();
        string connectionString = "region=us-west-2;accessKey=ACCESS_KEY_HERE;secretKey=SECRET_KEY_HERE";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("us-west-2", config.Region);
        Assert.Equal("ACCESS_KEY_HERE", config.AccessKey);
        Assert.Equal("SECRET_KEY_HERE", config.SecretKey);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutRegion_UsesDefaultRegion()
    {
        // Arrange
        var config = new OltConnectionConfigAmazon();
        string connectionString = "accessKey=ACCESS_KEY_HERE;secretKey=SECRET_KEY_HERE";
        string defaultRegion = "us-east-1";

        // Act
        config.Parse(connectionString, defaultRegion);

        // Assert
        Assert.Equal(defaultRegion, config.Region);
        Assert.Equal("ACCESS_KEY_HERE", config.AccessKey);
        Assert.Equal("SECRET_KEY_HERE", config.SecretKey);
    }

    [Fact]
    public void Parse_EmptyConnectionString_SetsPropertiesToNull()
    {
        // Arrange
        var config = new OltConnectionConfigAmazon();
        string connectionString = "";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Region);
        Assert.Null(config.AccessKey);
        Assert.Null(config.SecretKey);
    }

    [Theory]
    [InlineData("region=us-east-2;accessKey=987xyz;secretKey=abc123", "us-east-2", "987xyz", "abc123")]
    [InlineData("region=us-east-2;accessKey=987xyz;secretKey=", "us-east-2", "987xyz", null)]
    [InlineData("region=us-east-2;accessKey=987xyz", "us-east-2", "987xyz", null)]
    [InlineData("region=us-east-2;secretKey=abc123", "us-east-2", null, "abc123")]
    [InlineData("region=us-east-2;accessKey=;secretKey=abc123", "us-east-2", null, "abc123")]
    [InlineData("region=;accessKey=987xyz;secretKey=abc123", null, "987xyz", "abc123")]
    [InlineData("accessKey=987xyz;secretKey=abc123", null, "987xyz", "abc123")]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
    [InlineData("", null, null, null)]
    [InlineData(" ", null, null, null)]
    [InlineData(null, null, null, null)]
    public void Parse_ConnectionStrings(string? connectionString, string? expectedRegion, string? expectedUsername, string? expectedPassword)
    {
        var config = new OltConnectionConfigAmazon();
        config.Parse(connectionString);

        Assert.Equal(expectedRegion, config.Region);
        Assert.Equal(expectedUsername, config.AccessKey);
        Assert.Equal(expectedPassword, config.SecretKey);
    }

}
