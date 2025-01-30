namespace OLT.Core.Model.Abstractions.Tests;

public class OltSearcherGetAllTests
{
    public class TestEntity : OltEntityDeletable
    {
    }

    [Fact]
    public void BuildQueryable_ReturnsAllEntities()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new TestEntity(),
            new TestEntity(),
            new TestEntity(),
            new TestEntity { DeletedOn = DateTimeOffset.Now }
        }.AsQueryable();

        var searcher = new OltSearcherGetAll<TestEntity>();

        // Act
        var result = searcher.BuildQueryable(entities).ToList();

        // Assert
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Constructor_SetsIncludeDeletedCorrectly()
    {
        // Arrange
        bool includeDeleted = true;

        // Act
        var searcher = new OltSearcherGetAll<TestEntity>(includeDeleted);

        // Assert
        Assert.Equal(includeDeleted, searcher.IncludeDeleted);
    }

    [Fact]
    public void Searcher_IncludeDeleted_Check()
    {
        Assert.False(new OltSearcherGetAll<TestEntity>().IncludeDeleted);
        Assert.False(new OltSearcherGetAll<TestEntity>(false).IncludeDeleted);
        Assert.True(new OltSearcherGetAll<TestEntity>(true).IncludeDeleted);
    }

}

