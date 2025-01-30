using FluentAssertions;
using static OLT.Core.Model.Abstractions.Tests.TestHelper;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltPagedExtensionsTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    [Fact]
    public void ToPaged_ShouldReturnPagedResult()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 3, Name = "C" },
            new TestEntity { Id = 4, Name = "D" },
            new TestEntity { Id = 5, Name = "E" }
        }.AsQueryable();

        var pagingParams = new OltPagingParams
        {
            Page = 2,
            Size = 2
        };

        // Act
        var result = data.ToPaged(pagingParams);

        // Assert
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.Size);
        Assert.Equal(5, result.Count);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(3, result.Data.First().Id);
        Assert.Equal(4, result.Data.Last().Id);

        pagingParams = new OltPagingParams
        {
            Page = Faker.RandomNumber.Next(1, 4),
            Size = Faker.RandomNumber.Next(10, 20)
        };

        var list = TestPersonNameModel.FakerList(153);
        var queryable = list.AsQueryable();
        var expected = ExpectedPage(list, pagingParams);
        var paged = OltPagedExtensions.ToPaged(queryable, pagingParams);
        paged.Data.Should().BeEquivalentTo(expected);

    }

    private readonly Func<IQueryable<TestPersonNameModel>, IQueryable<TestPersonNameModel>> _defaultOrder = entity => entity.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst);

    [Fact]
    public void ToPaged_ShouldReturnPagedResultWithOrderBy()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 3, Name = "C" },
            new TestEntity { Id = 4, Name = "D" },
            new TestEntity { Id = 5, Name = "E" }
        }.AsQueryable();

        var pagingParams = new OltPagingParams
        {
            Page = 1,
            Size = 2
        };

        Func<IQueryable<TestEntity>, IQueryable<TestEntity>> orderBy = q => q.OrderBy(e => e.Name);

        // Act
        var result = data.ToPaged(pagingParams, orderBy);

        // Assert
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.Size);
        Assert.Equal(5, result.Count);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal(1, result.Data.First().Id);
        Assert.Equal(2, result.Data.Last().Id);

        pagingParams = new OltPagingParams
        {
            Page = Faker.RandomNumber.Next(1, 4),
            Size = Faker.RandomNumber.Next(10, 20)
        };

        var list = TestPersonNameModel.FakerList(153);
        var queryable = list.AsQueryable();
        var paged = OltPagedExtensions.ToPaged(queryable, pagingParams, _defaultOrder);

        var expected = ExpectedPage(_defaultOrder(list.AsQueryable()).ToList(), pagingParams);
        paged.Data.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());

    }

    [Fact]
    public void ToPaged_ShouldThrowArgumentNullException()
    {
        var list = TestPersonNameModel.FakerList(153);
        var queryable = list.AsQueryable();

        var @params = new OltPagingParams
        {
            Page = Faker.RandomNumber.Next(1, 4),
            Size = Faker.RandomNumber.Next(10, 20)
        };

        Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(null, null));
        Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(queryable, null));
        Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(null, @params));
    }

    private List<TestPersonNameModel> ExpectedPage(List<TestPersonNameModel> list, OltPagingParams pagingParams)
    {
        return list.Skip((pagingParams.Page - 1) * pagingParams.Size).Take(pagingParams.Size).ToList();
    }

}
