namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigSmtpTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsPropertiesCorrectly()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string connectionString = "host=localhost;port=587;username=usernameHere;password=passwordHere;ssl=true";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(587, config.Port);
        Assert.True(config.EnableSsl);
    }

    [Fact]
    public void Parse_EmptyConnectionString_SetsPropertiesToDefaults()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string connectionString = "";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
        Assert.Equal(587, config.Port); // Default port
        Assert.Null(config.EnableSsl);
    }

    [Fact]
    public void Parse_NullConnectionString_SetsPropertiesToDefaults()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string? connectionString = null;

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
        Assert.Equal(587, config.Port); // Default port
        Assert.Null(config.EnableSsl);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutPort_SetsPortToDefault()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string connectionString = "host=localhost;username=usernameHere;password=passwordHere;ssl=true";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(587, config.Port); // Default port
        Assert.True(config.EnableSsl);
    }

    [Fact]
    public void Parse_ConnectionStringWithInvalidPort_SetsPortToDefault()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string connectionString = "host=localhost;port=invalid;username=usernameHere;password=passwordHere;ssl=true";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(587, config.Port); // Default port
        Assert.True(config.EnableSsl);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutSsl_SetsEnableSslToNull()
    {
        // Arrange
        var config = new OltConnectionConfigSmtp();
        string connectionString = "host=localhost;port=587;username=usernameHere;password=passwordHere";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(587, config.Port);
        Assert.Null(config.EnableSsl);
    }


    [Theory]
    [InlineData("host=localhost;port=41;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 41, true)]
    [InlineData("host=localhost;port=41;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 41, false)]
    [InlineData("host=localhost;port=42;username=guest;password=abc123;workingdir=", "localhost", "guest", "abc123", 42, null)]
    [InlineData("host=localhost;port=43;username=guest;password=abc123", "localhost", "guest", "abc123", 43, null)]
    [InlineData("host=localhost;port=44;username=guest;password=;ssl=true", "localhost", "guest", null, 44, true)]
    [InlineData("host=localhost;port=44;username=guest;password=;ssl=false", "localhost", "guest", null, 44, false)]
    [InlineData("host=localhost;port=45;username=guest;ssl=true", "localhost", "guest", null, 45, true)]
    [InlineData("host=localhost;port=45;username=guest;ssl=false", "localhost", "guest", null, 45, false)]
    [InlineData("host=localhost;port=46;username=;password=abc123;ssl=true", "localhost", null, "abc123", 46, true)]
    [InlineData("host=localhost;port=46;username=;password=abc123;ssl=false", "localhost", null, "abc123", 46, false)]
    [InlineData("host=localhost;port=47;password=abc123;ssl=true", "localhost", null, "abc123", 47, true)]
    [InlineData("host=localhost;port=47;password=abc123;ssl=false", "localhost", null, "abc123", 47, false)]
    [InlineData("host=localhost;port=;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 587, true)]
    [InlineData("host=localhost;port=;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 587, false)]
    [InlineData("host=localhost;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 587, true)]
    [InlineData("host=localhost;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 587, false)]
    [InlineData("host=;port=48;username=guest;password=abc123;ssl=true", null, "guest", "abc123", 48, true)]
    [InlineData("host=;port=48;username=guest;password=abc123;ssl=false", null, "guest", "abc123", 48, false)]
    [InlineData("port=49;username=guest;password=abc123;ssl=true", null, "guest", "abc123", 49, true)]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null, 587, null)]
    [InlineData("host=localhost;port=9147483647;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 587, true)]  //Exceed int.MaxValue
    [InlineData("host=localhost;port=9147483647;username=guest;password=abc123;ssl=abc", "localhost", "guest", "abc123", 587, null)]  //Exceed int.MaxValue
    [InlineData("", null, null, null, 587, null)]
    [InlineData(" ", null, null, null, 587, null)]
    [InlineData(null, null, null, null, 587, null)]
    public void Parse_ConnectionStrings(string? connectionString, string? expectedHost, string? expectedUsername, string? expectedPassword, int? expectedPort, bool? expectedSsl)
    {
        var config = new OltConnectionConfigSmtp();
        config.Parse(connectionString);

        Assert.Equal(expectedHost, config.Host);
        Assert.Equal(expectedUsername, config.Username);
        Assert.Equal(expectedPassword, config.Password);
        Assert.Equal(expectedPort, config.Port);
        Assert.Equal(expectedSsl, config.EnableSsl);
    }
}
