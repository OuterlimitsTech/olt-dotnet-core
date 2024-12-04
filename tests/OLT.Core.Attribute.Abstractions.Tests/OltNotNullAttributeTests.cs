namespace OLT.Core.Attribute.Abstractions.Tests;

public class OltNotNullAttributeTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNotNull()
    {
        // Arrange
        var attribute = new OltNotNullAttribute();
        var value = new object();

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsNull()
    {
        // Arrange
        var attribute = new OltNotNullAttribute();
        object? value = null;

        // Act
        var result = attribute.IsValid(value);

        // Assert
        Assert.False(result);
    }
}
