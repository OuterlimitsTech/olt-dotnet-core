namespace OLT.Core.Attribute.Abstractions.Tests;

public class CodeAttributeTests
{
    [Fact]
    public void Constructor_ShouldInitializeCodeProperty()
    {
        // Arrange
        var code = "TestCode";

        // Act
        var attribute = new CodeAttribute(code);

        // Assert
        Assert.Equal(code, attribute.Code);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
        // Arrange
        string? code = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CodeAttribute(code));
    }
}
