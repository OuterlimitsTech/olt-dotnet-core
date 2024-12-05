namespace OLT.Core.Model.Abstractions.Tests;

public class OltGenericParameterTests
{
    [Fact]
    public void Constructor_SetsValuesCorrectly()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" }
        };

        // Act
        var parameter = new OltGenericParameter(values);

        // Assert
        Assert.Equal(values, parameter.Values);
    }

    [Fact]
    public void GetValue_ReturnsCorrectValue()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "123" },
            { "Key2", "Value2" }
        };
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue<int>("Key1");

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void GetValue_ReturnsDefaultValue_WhenKeyNotFound()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "123" }
        };
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue<int>("Key2");

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public void GetValue_WithDefaultValue_ReturnsCorrectValue()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "123" }
        };
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue("Key1", 456);

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void GetValue_WithDefaultValue_ReturnsDefaultValue_WhenKeyNotFound()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "123" }
        };
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue("Key2", 456);

        // Assert
        Assert.Equal(456, result);
    }

    [Fact]
    public void GetValue_String_ReturnsCorrectValue()
    {
        // Arrange
        var values = new Dictionary<string, string>
        {
            { "Key1", "Value1" }
        };
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue("Key1");

        // Assert
        Assert.Equal("Value1", result);
    }

    [Fact]
    public void GetValue_String_ReturnsNull_WhenKeyNotFound()
    {
        // Arrange
        var values = new Dictionary<string, string>();
        var parameter = new OltGenericParameter(values);

        // Act
        var result = parameter.GetValue("Key1");

        // Assert
        Assert.Null(result);
    }

    private class NullParameterStringParser : OltGenericParameterParser<string>
    {
        public NullParameterStringParser() : base(null)
        {
        }

        public override bool HasValue => false;

        public override bool Parse(IOltGenericParameter parameters)
        {
            return false;
        }
    }

    [Fact]
    //Test a null key for the OltGenericParameterParser
    public void Parser_ShowThrowException()
    {
        Assert.Throws<ArgumentNullException>("key", () => new NullParameterStringParser());
    }
}
