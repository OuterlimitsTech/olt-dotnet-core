namespace OLT.Core.Attribute.Abstractions.Tests;

public class KeyValueAttributeTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var key = "TestKey";
        var value = "TestValue";

        // Act
        var attribute = new KeyValueAttribute(key, value);

        // Assert
        Assert.Equal(key, attribute.Key);
        Assert.Equal(value, attribute.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        string? key = null;
        var value = "TestValue";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new KeyValueAttribute(key, value));
    }

    public enum TestValue
    {
        [KeyValue("key1", "value1")]
        Single,

        NoAttribute,

        [KeyValue("key1", "value1")]
        [KeyValue("key2", "value2")]
        [KeyValue("key3", "value3")]
        Muliple,

    }

    [Fact]
    public void KeyValueAttribute_ShouldInitializeProperties()
    {
        var key = Faker.Lorem.Words(9).Last();
        var value = Faker.Lorem.Words(15).Last();
        var attr = new KeyValueAttribute(key, value);
        Assert.Equal(key, attr.Key);
        Assert.Equal(value, attr.Value);
        Assert.Throws<ArgumentNullException>(() => new KeyValueAttribute(null, null));
        Assert.Throws<ArgumentNullException>(() => new KeyValueAttribute(null, value));
        Assert.Null(new KeyValueAttribute(key, null).Value);
    }

    [Theory]
    [InlineData(1, TestValue.Single)]
    [InlineData(0, TestValue.NoAttribute)]
    [InlineData(3, TestValue.Muliple)]
    public void GetKeyValueAttributes_ShouldReturnCorrectCount(int expected, TestValue @enum)
    {
        Assert.Equal(expected, OltKeyValueAttributeExtensions.GetKeyValueAttributes(@enum)?.Count);
    }
}
