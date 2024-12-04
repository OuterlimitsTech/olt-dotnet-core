namespace OLT.Core.Model.Abstractions.Tests;

public class OltSortParamsTests
{
    [Fact]
    public void PropertyName_ShouldReturnSetValue()
    {
        // Arrange
        var sortParams = new OltSortParams
        {
            PropertyName = "Name"
        };

        // Act
        var propertyName = sortParams.PropertyName;

        // Assert
        Assert.Equal("Name", propertyName);
    }

    [Fact]
    public void IsAscending_ShouldReturnSetValue()
    {
        // Arrange
        var sortParams = new OltSortParams
        {
            IsAscending = true
        };

        // Act
        var isAscending = sortParams.IsAscending;

        // Assert
        Assert.True(isAscending);
    }

    [Fact]
    public void IsAscending_ShouldReturnFalse_WhenSetToFalse()
    {
        // Arrange
        var sortParams = new OltSortParams
        {
            IsAscending = false
        };

        // Act
        var isAscending = sortParams.IsAscending;

        // Assert
        Assert.False(isAscending);
    }

    [Fact]
    public void OltSortParams_ShouldInitializeWithDefaultValues()
    {
        var model = new OltSortParams();
        Assert.False(model.IsAscending);
        Assert.Null(model.PropertyName);
        Assert.NotNull(model as IOltSortParams);

        var propName = Faker.Lorem.Words(11).Last();

        model.PropertyName = propName;
        model.IsAscending = true;

        Assert.Equal(propName, model.PropertyName);
        Assert.True(model.IsAscending);
    }
}
