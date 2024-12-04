using OLT.Constants;

namespace OLT.Core.Attribute.Abstractions.Tests;

public class OltSortOrderAttributeExtensionsTests
{
    private enum TestEnum
    {
        [SortOrder(1)]
        FirstValue,
        [SortOrder(987)]
        SecondValue,
        NoSortOrder
    }

    [Fact]
    public void GetSortOrderEnum_ReturnsCorrectSortOrder_WhenAttributeIsPresent()
    {
        // Arrange
        var enumValue = TestEnum.FirstValue;

        // Act
        var sortOrder = enumValue.GetSortOrderEnum();

        // Assert
        Assert.Equal(1, sortOrder);
    }

    [Fact]
    public void GetSortOrderEnum_ReturnsDefaultSortOrder_WhenAttributeIsNotPresent()
    {
        // Arrange
        var enumValue = TestEnum.NoSortOrder;
        short defaultSortOrder = OltCommonDefaults.SortOrder;

        // Act
        var sortOrder = enumValue.GetSortOrderEnum(defaultSortOrder);

        // Assert
        Assert.Equal(defaultSortOrder, sortOrder);
    }

    [Fact]
    public void GetSortOrderEnum_ReturnsProvidedDefaultSortOrder_WhenAttributeIsNotPresent()
    {
        // Arrange
        var enumValue = TestEnum.NoSortOrder;
        short providedDefaultSortOrder = 5;

        // Act
        var sortOrder = enumValue.GetSortOrderEnum(providedDefaultSortOrder);

        // Assert
        Assert.Equal(providedDefaultSortOrder, sortOrder);
    }

    [Fact]
    public void GetSortOrderEnum_ReturnsCorrectSortOrder_WhenAttributeIsPresentOnDifferentValue()
    {
        // Arrange
        var enumValue = TestEnum.SecondValue;

        // Act
        var sortOrder = enumValue.GetSortOrderEnum();

        // Assert
        Assert.Equal(987, sortOrder);
    }

    public enum SortAttributeTest
    {
        [SortOrder(15)]
        Value1,

        Value2,

        [SortOrder(987)]
        Value3,

        Value4,
    }


    [Theory]
    [InlineData((short)15, SortAttributeTest.Value1)]
    [InlineData(OltCommonDefaults.SortOrder, SortAttributeTest.Value2)]
    [InlineData((short)987, SortAttributeTest.Value3)]
    [InlineData(OltCommonDefaults.SortOrder, SortAttributeTest.Value4)]
    [InlineData(OltCommonDefaults.SortOrder, null)]
    public void GetSortOrderEnum_ReturnsExpectedSortOrder(short? expectedSort, SortAttributeTest? value)
    {
        Assert.Equal(expectedSort, OltSortOrderAttributeExtensions.GetSortOrderEnum(value));
    }

    [Theory]
    [InlineData((short)15, SortAttributeTest.Value1, (short)1987)]
    [InlineData((short)1765, SortAttributeTest.Value2, (short)1765)]
    [InlineData((short)987, SortAttributeTest.Value3, (short)1567)]
    [InlineData((short)1456, SortAttributeTest.Value4, (short)1456)]
    [InlineData((short)8766, null, (short)8766)]
    public void GetSortOrderEnum_ReturnsExpectedSortOrderWithDefault(short? expectedSort, SortAttributeTest? value, short defaultValue)
    {
        Assert.Equal(expectedSort, OltSortOrderAttributeExtensions.GetSortOrderEnum(value, defaultValue));
    }

}
