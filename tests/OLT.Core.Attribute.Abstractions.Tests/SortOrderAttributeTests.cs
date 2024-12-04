namespace OLT.Core.Attribute.Abstractions.Tests;

public class SortOrderAttributeTests
{
    [Fact]
    public void Constructor_Default_SetsSortOrderToDefault()
    {
        // Arrange & Act
        var attribute = new SortOrderAttribute();

        // Assert
        Assert.Equal(SortOrderAttribute.SortOrderDefault, attribute.SortOrder);
    }

    [Fact]
    public void Constructor_WithSortOrder_SetsSortOrder()
    {
        // Arrange
        short expectedSortOrder = 5;

        // Act
        var attribute = new SortOrderAttribute(expectedSortOrder);

        // Assert
        Assert.Equal(expectedSortOrder, attribute.SortOrder);
    }

    [Fact]
    public void SortOrder_Setter_UpdatesSortOrder()
    {
        // Arrange
        var attribute = new SortOrderAttribute();
        short newSortOrder = 10;

        // Act
        attribute.SortOrder = newSortOrder;

        // Assert
        Assert.Equal(newSortOrder, attribute.SortOrder);
    }
}
