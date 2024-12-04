namespace OLT.Core.Exception.Abstractions.Tests;

public class OltBadRequestExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Bad request exception message";

        // Act
        var exception = new OltBadRequestException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }
}
