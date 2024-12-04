using FluentAssertions;
using System.Linq.Expressions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionIntContainsTests
{
    public class TestEntity : IOltEntity
    {
        public int Id { get; set; }
    }

    [Fact]
    public void FieldExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        Expression<Func<TestEntity, int>> fieldExpression = e => e.Id;
        var expressionIntContains = new OltEntityExpressionIntContains<TestEntity>(fieldExpression);

        // Act
        var result = expressionIntContains.GetType().GetProperty("FieldExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(expressionIntContains);

        // Assert
        Assert.Equal(fieldExpression, result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        Expression<Func<TestEntity, int>> fieldExpression = e => e.Id;
        var expressionIntContains = new OltEntityExpressionIntContains<TestEntity>(fieldExpression)
        {
            Value = new List<int> { 1, 2, 3 }
        };

        // Act
        var whereExpression = expressionIntContains.GetType().GetProperty("WhereExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(expressionIntContains) as Expression<Func<int, bool>>;
        var compiledExpression = whereExpression.Compile();

        // Assert
        Assert.True(compiledExpression(1));
        Assert.False(compiledExpression(4));
    }

    [Fact]
    public void BuildQueryable_ShouldReturnFilteredQueryable()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 },
            new TestEntity { Id = 3 },
            new TestEntity { Id = 4 }
        }.AsQueryable();

        Expression<Func<TestEntity, int>> fieldExpression = e => e.Id;
        var expressionIntContains = new OltEntityExpressionIntContains<TestEntity>(fieldExpression)
        {
            Value = new List<int> { 2, 3 }
        };

        // Act
        var result = expressionIntContains.BuildQueryable(data);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.Id == 2);
        Assert.Contains(result, e => e.Id == 3);
    }


    [Fact]
    public void IntContains_ShouldReturnCorrectResults()
    {
        var expected = new List<EntityPersonModel>();
        expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(4, 8)));
        var list = TestHelper.BuildTestList(expected);

        var expression = new OltEntityExpressionIntContains<EntityPersonModel>(person => person.Id)
        {
            Value = expected.Select(s => s.Id).ToList()
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(expected.Count);
        results.Should().BeEquivalentTo(expected);
    }
}
