namespace OLT.Core.Model.Abstractions.Tests;

public class OltRouteParamsParserStringTests
{
    [Theory]
    [InlineData("test", true, "test")]
    [InlineData("  test  ", true, "  test  ")]
    [InlineData("", false, null)]
    [InlineData("   ", false, null)]
    [InlineData(null, false, null)]
    public void TryParse_ReturnsExpectedResult(string? input, bool expectedResult, string? expectedValue)
    {
        // Arrange
        var parser = new OltRouteParamsParserString();

        // Act
        var result = parser.TryParse(input, out var value);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedValue, value);
    }
}
