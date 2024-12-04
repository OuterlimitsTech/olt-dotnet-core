using FluentAssertions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionStringStartsWithTests
{    

    public class TestEntity : IOltEntity
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void WhereExpression_ShouldReturnTrue_WhenStringStartsWithValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "TestName" };
        var expression = new OltEntityExpressionStringStartsWith<TestEntity>(e => e.Name)
        {
            Value = "Test"
        };

        // Act
        var compiledExpression = expression.WhereExpression.Compile();
        var result = compiledExpression(entity.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnFalse_WhenStringDoesNotStartWithValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "TestName" };
        var expression = new OltEntityExpressionStringStartsWith<TestEntity>(e => e.Name)
        {
            Value = "Name"
        };

        // Act
        var compiledExpression = expression.WhereExpression.Compile();
        var result = compiledExpression(entity.Name);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnFalse_WhenStringIsNull()
    {
        // Arrange
        var entity = new TestEntity { Name = null };
        var expression = new OltEntityExpressionStringStartsWith<TestEntity>(e => e.Name)
        {
            Value = "Test"
        };

        // Act
        var compiledExpression = expression.WhereExpression.Compile();
        var result = compiledExpression(entity.Name);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void BuildQueryable_ShouldFilterEntitiesCorrectly()
    {
        // Arrange
        var entities = new[]
        {
            new TestEntity { Name = "TestName1" },
            new TestEntity { Name = "TestName2" },
            new TestEntity { Name = "OtherName" },
            new TestEntity { Name = null }
        }.AsQueryable();

        var expression = new OltEntityExpressionStringStartsWith<TestEntity>(e => e.Name)
        {
            Value = "Test"
        };

        // Act
        var filteredEntities = expression.BuildQueryable(entities).ToList();

        // Assert
        Assert.Equal(2, filteredEntities.Count);
        Assert.All(filteredEntities, e => Assert.StartsWith("Test", e.Name));
    }

    [Fact]
    public void StringStartsWith_ShouldReturnCorrectResults()
    {
        var firstName = nameof(StringStartsWith_ShouldReturnCorrectResults);

        var expected = new List<EntityPersonModel>();
        expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(4, 8)));
        expected.SetFirstNameStartsWith(firstName);

        var seedList = new List<EntityPersonModel>();
        seedList.AddRange(expected);

        for (int i = 0; i < Faker.RandomNumber.Next(5, 10); i++)
        {
            var edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameContains(firstName);
            seedList.Add(edgeTest);

            edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameEndsWith(firstName);
            seedList.Add(edgeTest);
        }


        var list = TestHelper.BuildTestList(seedList);

        var expression = new OltEntityExpressionStringStartsWith<EntityPersonModel>(person => person.FirstName)
        {
            Value = firstName
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(expected.Count);
        results.Should().BeEquivalentTo(expected);
    }
}
