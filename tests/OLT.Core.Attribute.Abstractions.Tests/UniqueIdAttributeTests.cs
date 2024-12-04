namespace OLT.Core.Attribute.Abstractions.Tests;

public class UniqueIdAttributeTests
{
    [Fact]
    public void Constructor_SetsUniqueId_WhenValidGuidStringProvided()
    {
        // Arrange
        var guidString = "d2719b1e-1c4b-4b8e-9b1e-1c4b4b8e9b1e";
        var expectedGuid = Guid.Parse(guidString);

        // Act
        var attribute = new UniqueIdAttribute(guidString);

        // Assert
        Assert.Equal(expectedGuid, attribute.UniqueId);
    }

    [Fact]
    public void Constructor_ThrowsFormatException_WhenInvalidGuidStringProvided()
    {
        // Arrange
        var invalidGuidString = "invalid-guid";

        // Act & Assert
        Assert.Throws<FormatException>(() => new UniqueIdAttribute(invalidGuidString));
    }

    public enum TestValue
    {
        [UniqueId("1393fff9-3850-4bb2-848b-18973a9f88d0")]
        Valid,

        NoAttrib,
    }

    [Theory]
    [InlineData("1393fff9-3850-4bb2-848b-18973a9f88d0", TestValue.Valid)]
    [InlineData(null, TestValue.NoAttrib)]
    public void GetUniqueIdFromEnumValue_ReturnsExpectedGuid(string? expected, TestValue value)
    {
        Guid? guid = null;
        try
        {
            guid = expected == null ? null : new Guid(expected);
        }
        catch
        {
            // ignore
        }

        var uid = OltAttributeExtensions.GetAttributeInstance<UniqueIdAttribute, TestValue>(value)?.UniqueId;
        Assert.Equal(guid, uid);
    }
}

