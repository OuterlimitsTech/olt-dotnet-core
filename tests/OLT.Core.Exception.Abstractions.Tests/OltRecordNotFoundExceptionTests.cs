namespace OLT.Core.Exception.Abstractions.Tests;

public class OltRecordNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Record not found exception message";

        // Act
        var exception = new OltRecordNotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    public enum TestServiceMessageEnum
    {
        [System.ComponentModel.Description("Test Message")]
        TestMessage
    }

    [Fact]
    public void Constructor_WithEnumMessage_ShouldSetFormattedMessage()
    {
        // Arrange
        var messageType = TestServiceMessageEnum.TestMessage;

        // Act
        var exception = new OltRecordNotFoundException<TestServiceMessageEnum>(messageType);

        // Assert
        Assert.Equal("Test Message Not Found", exception.Message);
    }
}

