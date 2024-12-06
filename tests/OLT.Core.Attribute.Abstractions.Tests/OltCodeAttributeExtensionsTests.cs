using System.ComponentModel;

namespace OLT.Core.Attribute.Abstractions.Tests;

public class OltCodeAttributeExtensionsTests
{
    private enum TestEnum
    {
        [Code("Code1")]
        ValueWithCode,

        ValueWithoutCode
    }

    [Fact]
    public void GetCodeEnum_ShouldReturnCode_WhenEnumValueHasCodeAttribute()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithCode;

        // Act
        var code = enumValue.GetCodeEnum();

        // Assert
        Assert.Equal("Code1", code);
    }

    [Fact]
    public void GetCodeEnum_ShouldReturnNull_WhenEnumValueHasNoCodeAttribute()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithoutCode;

        // Act
        var code = enumValue.GetCodeEnum();

        // Assert
        Assert.Null(code);
    }

    [Fact]
    public void GetCodeEnum_ShouldReturnNull_WhenEnumValueIsNull()
    {
        // Arrange
        TestEnum? enumValue = null;

        // Act
        var code = enumValue.GetCodeEnum();

        // Assert
        Assert.Null(code);
    }

    [Fact]
    public void GetCodeEnumSafe_ShouldReturnCode_WhenEnumValueHasCodeAttribute()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithCode;

        // Act
        var code = enumValue.GetCodeEnumSafe();

        // Assert
        Assert.Equal("Code1", code);
    }

    [Fact]
    public void GetCodeEnumSafe_ShouldThrowException_WhenEnumValueHasNoCodeAttribute()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithoutCode;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => enumValue.GetCodeEnumSafe());
    }

    [Fact]
    public void FromCodeEnum_ShouldReturnEnumValue_WhenCodeMatches()
    {
        // Arrange
        var code = "Code1";

        // Act
        var enumValue = code.FromCodeEnum<TestEnum>();

        // Assert
        Assert.Equal(TestEnum.ValueWithCode, enumValue);
    }

    [Fact]
    public void FromCodeEnum_ShouldReturnEnumValue_WhenNameMatches()
    {
        // Arrange
        var name = "ValueWithoutCode";

        // Act
        var enumValue = name.FromCodeEnum<TestEnum>();

        // Assert
        Assert.Equal(TestEnum.ValueWithoutCode, enumValue);
    }

    [Fact]
    public void FromCodeEnum_ShouldThrowArgumentException_WhenCodeOrNameDoesNotMatch()
    {
        // Arrange
        var invalidCode = "InvalidCode";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => invalidCode.FromCodeEnum<TestEnum>());
    }

    public enum CodeAttributeTest
    {
        [Code("value-1")]
        Value1,

        Value2,

        [Description("Value 3")]
        Value3,

        [Description("Value 4")]
        [Code("value-4")]
        Value4,
    }

    [Theory]
    [InlineData("value-1", CodeAttributeTest.Value1)]
    [InlineData(null, CodeAttributeTest.Value2)]
    [InlineData(null, CodeAttributeTest.Value3)]
    [InlineData("value-4", CodeAttributeTest.Value4)]
    [InlineData(null, null)]
    public void GetCodeEnumExtensions_ShouldReturnExpectedCode(string expected, CodeAttributeTest? value)
    {
        Assert.Equal(expected, OltCodeAttributeExtensions.GetCodeEnum(value));
    }


    [Theory]
    [InlineData(CodeAttributeTest.Value1, "value-1")]
    [InlineData(CodeAttributeTest.Value4, "value-4")]
    [InlineData(null, null)]
    public void FromCodeEnumExtensions_ShouldReturnExpectedEnumValue(CodeAttributeTest? expected, string value)
    {
        if (value == null)
        {
            Assert.Throws<System.ArgumentNullException>(() => OltCodeAttributeExtensions.FromCodeEnum<CodeAttributeTest>(value));
        }
        else
        {
            Assert.Equal(expected, OltCodeAttributeExtensions.FromCodeEnum<CodeAttributeTest>(value));
        }
    }

}
