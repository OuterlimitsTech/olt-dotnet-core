using AwesomeAssertions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltEntityExpressionStringEqualsTests
{
    private class TestEntity : IOltEntity
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void WhereExpression_ShouldReturnTrue_WhenStringEqualsValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "Hello World" };
        var expression = new OltEntityExpressionStringEquals<TestEntity>(e => e.Name)
        {
            Value = "Hello World"
        };

        // Act
        var whereExpression = expression.WhereExpression.Compile();
        var result = whereExpression(entity.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void WhereExpression_ShouldReturnFalse_WhenStringDoesNotEqualValue()
    {
        // Arrange
        var entity = new TestEntity { Name = "Hello World" };
        var expression = new OltEntityExpressionStringEquals<TestEntity>(e => e.Name)
        {
            Value = "Goodbye World"
        };

        // Act
        var whereExpression = expression.WhereExpression.Compile();
        var result = whereExpression(entity.Name);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StringEquals_ShouldReturnCorrectResults()
    {
        var firstName = nameof(StringEquals_ShouldReturnCorrectResults);

        var expected = new List<EntityPersonModel>();
        expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(4, 8)));
        expected.SetFirstName(firstName);

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

            edgeTest = EntityPersonModel.FakerData();
            edgeTest.SetFirstNameStartsWith(firstName);
            seedList.Add(edgeTest);
        }


        var list = TestHelper.BuildTestList(seedList);

        var expression = new OltEntityExpressionStringEquals<EntityPersonModel>(person => person.FirstName)
        {
            Value = firstName
        };
        var results = expression.BuildQueryable(list.AsQueryable()).ToList();
        results.Should().HaveCount(expected.Count);
        results.Should().BeEquivalentTo(expected);
    }

}
