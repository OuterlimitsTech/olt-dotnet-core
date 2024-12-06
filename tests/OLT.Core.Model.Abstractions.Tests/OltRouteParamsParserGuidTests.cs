namespace OLT.Core.Model.Abstractions.Tests;

public class OltRouteParamsParserGuidTests
{
    [Theory]
    [InlineData("d3b07384-d9a0-4f1b-8b0d-2b0b0b0b0b0b", true)]
    [InlineData("D3B07384-D9A0-4F1B-8B0D-2B0B0B0B0B0B", true)]
    [InlineData("invalid-guid", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void TryParse_ReturnsExpectedResult(string? input, bool expectedResult)
    {
        // Arrange
        var parser = new OltRouteParamsParserGuid();

        // Act
        var result = parser.TryParse(input, out var value);

        // Assert
        Assert.Equal(expectedResult, result);
        if (expectedResult)
        {
            Assert.NotEqual(Guid.Empty, value);
        }
        else
        {
            Assert.Equal(Guid.Empty, value);
        }
    }
}

