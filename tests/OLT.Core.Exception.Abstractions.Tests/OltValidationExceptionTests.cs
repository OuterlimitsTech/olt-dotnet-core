namespace OLT.Core.Exception.Abstractions.Tests;

public class OltValidationExceptionTests
{
    [Fact]
    public void Constructor_WithResults_ShouldSetResultsAndDefaultMessage()
    {
        // Arrange
        var validationErrors = new List<IOltValidationError>
        {
            new OltValidationError("Error 1"),
            new OltValidationError("Error 2")
        };

        // Act
        var exception = new OltValidationException(validationErrors);

        // Assert
        Assert.Equal("Please correct the validation errors", exception.Message);
        Assert.Equal(validationErrors, exception.Results);
    }

    [Fact]
    public void Constructor_WithResultsAndCustomMessage_ShouldSetResultsAndCustomMessage()
    {
        // Arrange
        var validationErrors = new List<IOltValidationError>
        {
            new OltValidationError("Error 1"),
            new OltValidationError("Error 2")
        };
        var customMessage = "Custom validation error message";

        // Act
        var exception = new OltValidationException(validationErrors, customMessage);

        // Assert
        Assert.Equal(customMessage, exception.Message);
        Assert.Equal(validationErrors, exception.Results);
    }
}

