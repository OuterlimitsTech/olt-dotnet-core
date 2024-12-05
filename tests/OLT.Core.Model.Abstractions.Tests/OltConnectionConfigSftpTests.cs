namespace OLT.Core.Model.Abstractions.Tests;

public class OltConnectionConfigSftpTests
{
    [Fact]
    public void Parse_ValidConnectionString_SetsPropertiesCorrectly()
    {
        // Arrange
        var config = new OltConnectionConfigSftp();
        string connectionString = "host=localhost;port=22;username=usernameHere;password=passwordHere;workingdir=/root/test";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(22, config.Port);
        Assert.Equal("/root/test", config.WorkingDirectory);
    }

    [Fact]
    public void Parse_EmptyConnectionString_SetsPropertiesToDefaults()
    {
        // Arrange
        var config = new OltConnectionConfigSftp();
        string connectionString = "";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
        Assert.Equal(22, config.Port); // Default port
        Assert.Null(config.WorkingDirectory);
    }

    [Fact]
    public void Parse_NullConnectionString_SetsPropertiesToDefaults()
    {
        // Arrange
        var config = new OltConnectionConfigSftp();
        string? connectionString = null;

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Null(config.Host);
        Assert.Null(config.Username);
        Assert.Null(config.Password);
        Assert.Equal(22, config.Port); // Default port
        Assert.Null(config.WorkingDirectory);
    }

    [Fact]
    public void Parse_ConnectionStringWithoutPort_SetsPortToDefault()
    {
        // Arrange
        var config = new OltConnectionConfigSftp();
        string connectionString = "host=localhost;username=usernameHere;password=passwordHere;workingdir=/root/test";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(22, config.Port); // Default port
        Assert.Equal("/root/test", config.WorkingDirectory);
    }

    [Fact]
    public void Parse_ConnectionStringWithInvalidPort_SetsPortToDefault()
    {
        // Arrange
        var config = new OltConnectionConfigSftp();
        string connectionString = "host=localhost;port=invalid;username=usernameHere;password=passwordHere;workingdir=/root/test";

        // Act
        config.Parse(connectionString);

        // Assert
        Assert.Equal("localhost", config.Host);
        Assert.Equal("usernameHere", config.Username);
        Assert.Equal("passwordHere", config.Password);
        Assert.Equal(22, config.Port); // Default port
        Assert.Equal("/root/test", config.WorkingDirectory);
    }


    [Theory]
    [InlineData("host=localhost;port=41;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 41, "/root/test")]
    [InlineData("host=localhost;port=42;username=guest;password=abc123;workingdir=", "localhost", "guest", "abc123", 42, null)]
    [InlineData("host=localhost;port=43;username=guest;password=abc123", "localhost", "guest", "abc123", 43, null)]
    [InlineData("host=localhost;port=44;username=guest;password=;workingdir=/root/test", "localhost", "guest", null, 44, "/root/test")]
    [InlineData("host=localhost;port=45;username=guest;workingdir=/root/test", "localhost", "guest", null, 45, "/root/test")]
    [InlineData("host=localhost;port=46;username=;password=abc123;workingdir=/root/test", "localhost", null, "abc123", 46, "/root/test")]
    [InlineData("host=localhost;port=47;password=abc123;workingdir=/root/test", "localhost", null, "abc123", 47, "/root/test")]
    [InlineData("host=localhost;port=;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 22, "/root/test")]
    [InlineData("host=localhost;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 22, "/root/test")]
    [InlineData("host=;port=48;username=guest;password=abc123;workingdir=/root/test", null, "guest", "abc123", 48, "/root/test")]
    [InlineData("port=49;username=guest;password=abc123;workingdir=/root/test", null, "guest", "abc123", 49, "/root/test")]
    [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null, 22, null)]
    [InlineData("host=localhost;port=9147483647;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 22, "/root/test")] //Exceed int.MaxValue
    [InlineData("", null, null, null, 22, null)]
    [InlineData(" ", null, null, null, 22, null)]
    [InlineData(null, null, null, null, 22, null)]
    public void Parse_ConnectionStrings(string connectionString, string expectedHost, string expectedUsername, string expectedPassword, int? expectedPort, string expectedWorkingDir)
    {
        var config = new OltConnectionConfigSftp();
        config.Parse(connectionString);

        Assert.Equal(expectedHost, config.Host);
        Assert.Equal(expectedUsername, config.Username);
        Assert.Equal(expectedPassword, config.Password);
        Assert.Equal(expectedPort, config.Port);
        Assert.Equal(expectedWorkingDir, config.WorkingDirectory);
    }
}
