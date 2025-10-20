using AwesomeAssertions;
using Moq;
using static OLT.Core.Model.Abstractions.Tests.TestHelper;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltOrderByExtensionsTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    [Fact]
    public void OrderByPropertyName_ShouldOrderByPropertyNameAscending()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 3, Name = "C" }
        }.AsQueryable();

        // Act
        var result = data.OrderByPropertyName("Name", true).ToList();

        // Assert
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void OrderByPropertyName_ShouldOrderByPropertyNameDescending()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 3, Name = "C" }
        }.AsQueryable();

        // Act
        var result = data.OrderByPropertyName("Name", false).ToList();

        // Assert
        Assert.Equal(3, result[0].Id);
        Assert.Equal(2, result[1].Id);
        Assert.Equal(1, result[2].Id);
    }

    [Fact]
    public void OrderBy_ShouldOrderBySortParams()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 3, Name = "C" }
        }.AsQueryable();

        var sortParamsMock = new Mock<IOltSortParams>();
        sortParamsMock.SetupGet(sp => sp.PropertyName).Returns("Name");
        sortParamsMock.SetupGet(sp => sp.IsAscending).Returns(true);

        // Act
        var result = data.OrderBy(sortParamsMock.Object).ToList();

        // Assert
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void OrderBy_ShouldUseDefaultOrderByWhenSortParamsIsNull()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 3, Name = "C" }
        }.AsQueryable();

        Func<IQueryable<TestEntity>, IQueryable<TestEntity>> defaultOrderBy = q => q.OrderBy(e => e.Id);

        // Act
        var result = data.OrderBy(null, defaultOrderBy).ToList();

        // Assert
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
        Assert.Equal(3, result[2].Id);
    }



    

    private readonly Func<IQueryable<TestPersonNameModel>, IQueryable<TestPersonNameModel>> _defaultOrder = entity => entity.OrderBy(p => p.NameFirst);

    [Fact]
    public void OrderByPropertyName_ShouldOrderByNameLast()
    {
        var list = TestPersonNameModel.FakerList(25);
        var queryable = list.AsQueryable();

        OltOrderByExtensions.OrderByPropertyName(queryable, nameof(TestPersonNameModel.NameLast), true)
            .Should()
            .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

        OltOrderByExtensions.OrderByPropertyName(queryable, nameof(TestPersonNameModel.NameLast), false)
            .Should()
            .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBySortParams_ShouldOrderByNameLast()
    {
        var list = TestPersonNameModel.FakerList(25);
        var queryable = list.AsQueryable();

        var sort = new OltSortParams
        {
            PropertyName = nameof(TestPersonNameModel.NameLast),
            IsAscending = true,
        };

        OltOrderByExtensions.OrderBy(queryable, sort)
            .Should()
            .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

        sort = new OltSortParams
        {
            PropertyName = nameof(TestPersonNameModel.NameLast),
            IsAscending = false,
        };

        OltOrderByExtensions.OrderBy(queryable, sort)
            .Should()
            .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderBySortParamsWithDefault_ShouldOrderByNameLastOrDefault()
    {
        var list = TestPersonNameModel.FakerList(25);
        var queryable = list.AsQueryable();

        var sort = new OltSortParams
        {
            PropertyName = nameof(TestPersonNameModel.NameLast),
            IsAscending = false,
        };

        OltOrderByExtensions.OrderBy(queryable, sort, _defaultOrder)
            .Should()
            .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());

        OltOrderByExtensions.OrderBy(queryable, null, _defaultOrder)
            .Should()
            .BeEquivalentTo(list.OrderBy(p => p.NameFirst), options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderByExceptions_ShouldThrowArgumentNullException()
    {
        var list = TestPersonNameModel.FakerList(25);
        var queryable = list.AsQueryable();
        var sort = new OltSortParams
        {
            PropertyName = nameof(TestPersonNameModel.NameLast),
            IsAscending = false,
        };

        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, null));
        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, sort));

        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, null, null));
        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(null, null, _defaultOrder));
        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(null, sort, _defaultOrder));
        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(queryable, sort, null));
        Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(queryable, null, null));
    }

}
