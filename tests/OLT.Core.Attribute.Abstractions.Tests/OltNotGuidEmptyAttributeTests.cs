namespace OLT.Core.Attribute.Abstractions.Tests;

public class OltNotGuidEmptyAttributeTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNull()
    {
        // Arrange
        var attribute = new OltNotGuidEmptyAttribute();
        object? value = null;

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNotGuid()
    {
        // Arrange
        var attribute = new OltNotGuidEmptyAttribute();
        var value = "not a guid";

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsGuidEmpty()
    {
        // Arrange
        var attribute = new OltNotGuidEmptyAttribute();
        var value = Guid.Empty;

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsGuidNotEmpty()
    {
        // Arrange
        var attribute = new OltNotGuidEmptyAttribute();
        var value = Guid.NewGuid();

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.True(result);
    }
}

