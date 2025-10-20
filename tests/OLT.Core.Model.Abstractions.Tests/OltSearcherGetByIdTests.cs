using AwesomeAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltSearcherGetByIdTests
{
    public class TestEntity : OltEntityIdDeletable
    {
        
    }

    [Fact]
    public void BuildQueryable_FiltersById()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 },
            new TestEntity { Id = 3 }
        }.AsQueryable();

        var searcher = new OltSearcherGetById<TestEntity>(2);

        // Act
        var result = searcher.BuildQueryable(entities).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        int id = 5;
        bool includeDeleted = true;

        // Act
        var searcher = new OltSearcherGetById<TestEntity>(id, includeDeleted);

        // Assert
        Assert.Equal(id, searcher.Id);
        Assert.Equal(includeDeleted, searcher.IncludeDeleted);
    }

    [Fact]
    public void Searcher_IncludeDeleted_Check()
    {
        Assert.False(new OltSearcherGetById<TestEntity>(1).IncludeDeleted);
        Assert.False(new OltSearcherGetById<TestEntity>(1, false).IncludeDeleted);
        Assert.True(new OltSearcherGetById<TestEntity>(1, true).IncludeDeleted);

    }

}
