using AwesomeAssertions;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltPagedSearchJsonTests
{
    [Fact]
    public void OltPagedSearchJson_Should_Set_And_Get_Properties()
    {
        // Arrange
        var pagedSearchJson = new OltPagedSearchJson<TestModel, TestCriteria>
        {
            Key = "testKey",
            Criteria = new TestCriteria(),
            Size = 10,
            Page = 1,
            Count = 100,
            Data = new List<TestModel> { new TestModel() }
        };

        // Act & Assert
        Assert.Equal("testKey", pagedSearchJson.Key);
        Assert.NotNull(pagedSearchJson.Criteria);
        Assert.Equal(10, pagedSearchJson.Size);
        Assert.Equal(1, pagedSearchJson.Page);
        Assert.Equal(100, pagedSearchJson.Count);
        Assert.Single(pagedSearchJson.Data);
    }

    [Fact]
    public void OltPagedSearchJson_Should_Initialize_And_Validate_Properties()
    {
        var criteria = new TestCriteria
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
        };
        var key = Guid.NewGuid().ToString();

        var model = new OltPagedSearchJson<TestModel, TestCriteria>();
        Assert.Null(model.Criteria);
        Assert.Empty(model.Data);
        Assert.Null(model.Key);
        Assert.Null(model as IOltPagingParams);
        Assert.NotNull(model as IOltPaged);
        Assert.NotNull(model as IOltPaged<TestModel>);
        Assert.NotNull(model as OltPagedJson<TestModel>);

        model.Key = key;
        model.Criteria = criteria;
        model.Criteria.Should().BeEquivalentTo(criteria);
        Assert.Equal(key, model.Key);
    }

    public class TestModel
    {
        public string? Name { get; set; }
        public string? StreetAddress { get; set; }
    }

    public class TestCriteria
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

}
