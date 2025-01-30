using FluentAssertions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionStringContainsTests
{
    private class TestEntity : IOltEntity
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void WhereExpression_ShouldReturnTrue_WhenStringContainsValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "Hello World" };
        var expression = new OltEntityExpressionStringContains<TestEntity>(e => e.Name)
        {
            Value = "World"
        };

        // Act
        var whereExpression = expression.WhereExpression.Compile();
        var result = whereExpression(entity.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnFalse_WhenStringDoesNotContainValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "Hello World" };
        var expression = new OltEntityExpressionStringContains<TestEntity>(e => e.Name)
        {
            Value = "Universe"
        };

        // Act
        var whereExpression = expression.WhereExpression.Compile();
        var result = whereExpression(entity.Name);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnExpectedResults_WhenStringContainsValue()
    {
        var firstName = nameof(WhereExpression_ShouldReturnExpectedResults_WhenStringContainsValue);
        var expected = new List<EntityPersonModel>();

        for (int i = 0; i < Faker.RandomNumber.Next(10, 40); i++)
        {
            var edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameContains(firstName);
            expected.Add(edgeTest);

            edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameEndsWith(firstName);
            expected.Add(edgeTest);

            edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameStartsWith(firstName);
            expected.Add(edgeTest);
        }

        var list = TestHelper.BuildTestList(expected);

        var expression = new OltEntityExpressionStringContains<EntityPersonModel>(person => person.FirstName)
        {
            Value = firstName
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(expected.Count);
        results.Should().BeEquivalentTo(expected);
    }

}
