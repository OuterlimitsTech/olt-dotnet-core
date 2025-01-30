namespace OLT.Core.Model.Abstractions.Tests;

public class OltPagingParamsTests
{
    [Fact]
    public void Page_ShouldReturnDefault_WhenNotSet()
    {
        // Arrange
        var pagingParams = new OltPagingParams();

        // Act
        var page = pagingParams.Page;

        // Assert
        Assert.Equal(1, page);
    }

    [Fact]
    public void Page_ShouldReturnSetValue_WhenSet()
    {
        // Arrange
        var pagingParams = new OltPagingParams
        {
            Page = 5
        };

        // Act
        var page = pagingParams.Page;

        // Assert
        Assert.Equal(5, page);
    }

    [Fact]
    public void Page_ShouldReturnMinimumValue_WhenSetToLessThanOne()
    {
        // Arrange
        var pagingParams = new OltPagingParams
        {
            Page = -1
        };

        // Act
        var page = pagingParams.Page;

        // Assert
        Assert.Equal(1, page);
    }

    [Fact]
    public void Size_ShouldReturnDefault_WhenNotSet()
    {
        // Arrange
        var pagingParams = new OltPagingParams();

        // Act
        var size = pagingParams.Size;

        // Assert
        Assert.Equal(10, size);
    }

    [Fact]
    public void Size_ShouldReturnSetValue_WhenSet()
    {
        // Arrange
        var pagingParams = new OltPagingParams
        {
            Size = 20
        };

        // Act
        var size = pagingParams.Size;

        // Assert
        Assert.Equal(20, size);
    }

    [Fact]
    public void Size_ShouldReturnMinimumValue_WhenSetToLessThanOne()
    {
        // Arrange
        var pagingParams = new OltPagingParams
        {
            Size = 0
        };

        // Act
        var size = pagingParams.Size;

        // Assert
        Assert.Equal(1, size);
    }

    [Fact]
    public void PagingParams_ShouldInitializeAndSetValuesCorrectly()
    {
        var model = new OltPagingParams();
        Assert.Equal(1, model.Page);
        Assert.Equal(10, model.Size);
        Assert.NotNull(model as IOltPagingParams);
        Assert.NotNull(model as IOltPaged);

        var page = Faker.RandomNumber.Next();
        var size = Faker.RandomNumber.Next();
        model.Page = page;
        model.Size = size;

        Assert.Equal(page, model.Page);
        Assert.Equal(size, model.Size);
    }

}
