using AwesomeAssertions;
using System.Linq.Expressions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionIntTests
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
        var expressionInt = new OltEntityExpressionInt<TestEntity>(fieldExpression);

        // Act
        var result = expressionInt.FieldExpression;

        // Assert
        Assert.Equal(fieldExpression, result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        Expression<Func<TestEntity, int>> fieldExpression = e => e.Id;
        var expressionInt = new OltEntityExpressionInt<TestEntity>(fieldExpression)
        {
            Value = 5
        };

        // Act
        var whereExpression = expressionInt.WhereExpression;
        var compiledExpression = whereExpression.Compile();

        // Assert
        Assert.True(compiledExpression(5));
        Assert.False(compiledExpression(3));
    }

    [Fact]
    public void BuildQueryable_ShouldReturnFilteredQueryable()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1 },
            new TestEntity { Id = 2 },
            new TestEntity { Id = 3 }
        }.AsQueryable();

        Expression<Func<TestEntity, int>> fieldExpression = e => e.Id;
        var expressionInt = new OltEntityExpressionInt<TestEntity>(fieldExpression)
        {
            Value = 2
        };

        // Act
        var result = expressionInt.BuildQueryable(data);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result.First().Id);
    }

    [Fact]
    public void Int_ShouldReturnCorrectResults()
    {
        var expected = EntityPersonModel.FakerData();
        var list = TestHelper.BuildTestList(expected);

        var expression = new OltEntityExpressionInt<EntityPersonModel>(person => person.Id)
        {
            Value = expected.Id
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(1);
        results.FirstOrDefault().Should().BeEquivalentTo(expected);
    }

}
