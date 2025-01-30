namespace OLT.Core.Model.Abstractions.Tests;

public class OltSearcherGetByUidTests
{

    public class TestEntity : OltEntityId, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }
    }

    [Fact]
    public void BuildQueryable_FiltersByUid()
    {
        // Arrange
        var uid = Guid.NewGuid();
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, UniqueId = Guid.NewGuid() },
            new TestEntity { Id = 2, UniqueId = uid },
            new TestEntity { Id = 3, UniqueId = Guid.NewGuid() }
        }.AsQueryable();

        var searcher = new OltSearcherGetByUid<TestEntity>(uid);

        // Act
        var result = searcher.BuildQueryable(entities).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(uid, result[0].UniqueId);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var uid = Guid.NewGuid();
        bool includeDeleted = true;

        // Act
        var searcher = new OltSearcherGetByUid<TestEntity>(uid, includeDeleted);

        // Assert
        Assert.Equal(uid, searcher.Uid);
        Assert.Equal(includeDeleted, searcher.IncludeDeleted);
    }

    [Fact]
    public void Searcher_IncludeDeleted_Check()
    {
        Assert.False(new OltSearcherGetByUid<TestEntity>(Guid.Empty).IncludeDeleted);
        Assert.False(new OltSearcherGetByUid<TestEntity>(Guid.Empty, false).IncludeDeleted);
        Assert.True(new OltSearcherGetByUid<TestEntity>(Guid.Empty, true).IncludeDeleted);
    }

}
