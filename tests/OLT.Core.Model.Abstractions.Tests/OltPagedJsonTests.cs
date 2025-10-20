using AwesomeAssertions;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltPagedJsonTests
{
    [Fact]
    public void Page_ShouldReturnSetValue()
    {
        // Arrange
        var pagedJson = new OltPagedJson<string>
        {
            Page = 2
        };

        // Act
        var page = pagedJson.Page;

        // Assert
        Assert.Equal(2, page);
    }

    [Fact]
    public void Size_ShouldReturnSetValue()
    {
        // Arrange
        var pagedJson = new OltPagedJson<string>
        {
            Size = 20
        };

        // Act
        var size = pagedJson.Size;

        // Assert
        Assert.Equal(20, size);
    }

    [Fact]
    public void Count_ShouldReturnSetValue()
    {
        // Arrange
        var pagedJson = new OltPagedJson<string>
        {
            Count = 100
        };

        // Act
        var count = pagedJson.Count;

        // Assert
        Assert.Equal(100, count);
    }

    [Fact]
    public void Data_ShouldReturnSetValue()
    {
        // Arrange
        var data = new List<string> { "Item1", "Item2", "Item3" };
        var pagedJson = new OltPagedJson<string>
        {
            Data = data
        };

        // Act
        var result = pagedJson.Data;

        // Assert
        Assert.Equal(data, result);
    }

    [Fact]
    public void Data_ShouldReturnEmptyEnumerable_WhenNotSet()
    {
        // Arrange
        var pagedJson = new OltPagedJson<string>();

        // Act
        var result = pagedJson.Data;

        // Assert
        Assert.Empty(result);
    }

    public class TestPersonModel
    {
        public string? Name { get; set; }
        public string? StreetAddress { get; set; }
    }

    [Fact]
    public void PagedJson_ShouldInitializeAndSetValuesCorrectly()
    {
        var list = new List<TestPersonModel>();
        for (int i = 1; i <= Faker.RandomNumber.Next(18, 134); i++)
        {
            list.Add(new TestPersonModel
            {
                Name = Faker.Lorem.Words(i).Last(),
                StreetAddress = Faker.Lorem.Paragraphs(i).Last()
            });
        }

        var model = new OltPagedJson<TestPersonModel>();
        Assert.Equal(0, model.Page);
        Assert.Equal(0, model.Size);
        Assert.Equal(0, model.Count);


        Assert.Null(model as IOltPagingParams);
        Assert.NotNull(model as IOltPaged);
        Assert.NotNull(model as IOltPaged<TestPersonModel>);

        var page = Faker.RandomNumber.Next();
        var size = Faker.RandomNumber.Next();
        var count = Faker.RandomNumber.Next();
        var sortBy = Faker.Lorem.Words(10).Last();

        model.Page = page;
        model.Size = size;
        model.Count = count;
        model.Data = list;

        Assert.Equal(page, model.Page);
        Assert.Equal(size, model.Size);
        Assert.Equal(count, model.Count);
        model.Data.Should().BeEquivalentTo(list);
    }

}

