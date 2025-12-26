using AwesomeAssertions;
using System.Linq.Expressions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionIntNullableTests
{
    public class TestEntity : IOltEntity
    {
        public int? Id { get; set; }
    }

    [Fact]
    public void FieldExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        Expression<Func<TestEntity, int?>> fieldExpression = e => e.Id;
        var expressionIntNullable = new OltEntityExpressionIntNullable<TestEntity>(fieldExpression);

        // Act
        var result = expressionIntNullable.FieldExpression;

        // Assert
        Assert.Equal(fieldExpression, result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        Expression<Func<TestEntity, int?>> fieldExpression = e => e.Id;
        var expressionIntNullable = new OltEntityExpressionIntNullable<TestEntity>(fieldExpression)
        {
            Value = 5
        };

        // Act
        var whereExpression = expressionIntNullable.WhereExpression;
        var compiledExpression = whereExpression.Compile();

        // Assert
        Assert.True(compiledExpression(5));
        Assert.False(compiledExpression(3));
        Assert.False(compiledExpression(null));
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
            new TestEntity { Id = null }
        }.AsQueryable();

        Expression<Func<TestEntity, int?>> fieldExpression = e => e.Id;
        var expressionIntNullable = new OltEntityExpressionIntNullable<TestEntity>(fieldExpression)
        {
            Value = 2
        };

        // Act
        var result = expressionIntNullable.BuildQueryable(data);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result.First().Id);
    }

    [Fact]
    public void IntNullable_ShouldReturnCorrectResults()
    {
        var expected = EntityPersonModel.FakerData();
        var list = TestHelper.BuildTestList(expected);

        var expression = new OltEntityExpressionIntNullable<EntityPersonModel>(person => person.Id)
        {
            Value = expected.Id
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(1);
        results.FirstOrDefault().Should().BeEquivalentTo(expected);


        expression = new OltEntityExpressionIntNullable<EntityPersonModel>(person => person.Id)
        {
            Value = null
        };
        results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(0);
        results.FirstOrDefault().Should().BeNull();

    }

}
