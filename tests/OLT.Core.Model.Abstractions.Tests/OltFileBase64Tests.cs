namespace OLT.Core.Model.Abstractions.Tests;

public class OltFileBase64Tests
{
    [Fact]
    public void Success_ShouldReturnTrue_WhenFileBase64IsNotEmpty()
    {
        // Arrange
        var file = new OltFileBase64
        {
            FileBase64 = "dGVzdA==", // "test" in base64
            FileName = "test.txt",
            ContentType = "text/plain"
        };

        // Act
        var result = file.Success;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Success_ShouldReturnFalse_WhenFileBase64IsEmpty()
    {
        // Arrange
        var file = new OltFileBase64
        {
            FileBase64 = "",
            FileName = "test.txt",
            ContentType = "text/plain"
        };

        // Act
        var result = file.Success;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Success_ShouldReturnFalse_WhenFileBase64IsWhitespace()
    {
        // Arrange
        var file = new OltFileBase64
        {
            FileBase64 = "  ",
            FileName = "test.txt",
            ContentType = "text/plain"
        };

        // Act
        var result = file.Success;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Success_ShouldReturnFalse_WhenFileBase64IsNull()
    {
        // Arrange
        var file = new OltFileBase64
        {
            FileBase64 = null,
            FileName = "test.txt",
            ContentType = "text/plain"
        };

        // Act
        var result = file.Success;

        // Assert
        Assert.False(result);
    }
}
