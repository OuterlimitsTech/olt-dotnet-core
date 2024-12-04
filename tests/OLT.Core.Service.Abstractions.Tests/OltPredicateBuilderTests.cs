using FluentAssertions;
using System.Linq.Expressions;

namespace OLT.Core.Service.Abstractions.Tests;

public class OltPredicateBuilderTests
{
    private class TestEntity : IOltEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class TestOltEntityExpression : IOltEntityExpression<TestEntity, string>
    {
        public Expression<Func<string, bool>> WhereExpression { get; set; } = default!;
        public Expression<Func<TestEntity, string>> FieldExpression { get; set; } = default!;
        public string Value { get; set; } = string.Empty;
        public IQueryable<TestEntity> BuildQueryable(IQueryable<TestEntity> queryable) => queryable;
    }


    [Fact]
    public void And_ComposesTwoExpressionsWithAnd()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Id > 1;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name.Contains("Test");

        // Act
        var combined = expr1.And(expr2);

        // Assert
        var compiled = combined.Compile();
        Assert.True(compiled(new TestEntity { Id = 2, Name = "TestName" }));
        Assert.False(compiled(new TestEntity { Id = 0, Name = "TestName" }));
        Assert.False(compiled(new TestEntity { Id = 2, Name = "Name" }));
    }

    [Fact]
    public void Or_ComposesTwoExpressionsWithOr()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Id > 1;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name.Contains("Test");

        // Act
        var combined = expr1.Or(expr2);

        // Assert
        var compiled = combined.Compile();
        Assert.True(compiled(new TestEntity { Id = 2, Name = "TestName" }));
        Assert.True(compiled(new TestEntity { Id = 0, Name = "TestName" }));
        Assert.True(compiled(new TestEntity { Id = 2, Name = "Name" }));
        Assert.False(compiled(new TestEntity { Id = 0, Name = "Name" }));
    }

    [Fact]
    public void BuildExpression_CreatesCorrectExpression()
    {
        // Arrange
        var dynamicFilter = new TestOltEntityExpression
        {
            FieldExpression = e => e.Name,
            WhereExpression = value => value.Contains("Test")
        };

        // Act
        var expression = OltPredicateBuilder.BuildExpression(dynamicFilter);

        // Assert
        var compiled = expression.Compile();
        Assert.True(compiled(new TestEntity { Name = "TestName" }));
        Assert.False(compiled(new TestEntity { Name = "Name" }));
    }

    [Fact]
    public void BuildExpression_Or()
    {
        var firstNames = new List<string>
        {
            $"{nameof(BuildExpression_Or)}{Guid.NewGuid()}",
            $"{nameof(BuildExpression_Or)}{Guid.NewGuid()}",
            $"{nameof(BuildExpression_Or)}{Guid.NewGuid()}",
            $"{nameof(BuildExpression_Or)}{Guid.NewGuid()}"
        };

        var expected = EntityPersonModel.FakerList(4);
        for (var i = 0; i < firstNames.Count; i++)
        {
            expected[i].FirstName = firstNames[i];
        }
        var list = TestHelper.BuildTestList(expected, 1000, 7, 12);
        var queryable = list.AsQueryable();

        Expression<Func<EntityPersonModel, bool>>? expression = null;

        firstNames.ForEach(firstName =>
        {
            if (expression == null)
            {
                expression = p => p.FirstName == firstName;
            }
            else
            {
                expression = expression.Or(p => p.FirstName == firstName);
            }
        });

        var results = queryable.Where(expression).ToList();
        results.Should().HaveCount(firstNames.Count);
        results.Should().BeEquivalentTo(expected);

    }

    [Fact]
    public void BuildExpression_And()
    {
        var firstNamePrefix = nameof(BuildExpression_And);
        var person = EntityPersonModel.FakerData();


        var expected = EntityPersonModel.FakerList(4);
        expected.Add(person);

        expected.SetFirstNameStartsWith(firstNamePrefix);

        var list = TestHelper.BuildTestList(expected, 1000, 7, 12);
        var queryable = list.AsQueryable();

        Expression<Func<EntityPersonModel, bool>> expression = p => p.DeletedOn == null;
        expression = expression.And(p => p.FirstName.StartsWith(firstNamePrefix));

        var results = queryable.Where(expression).ToList();
        results.Should().HaveCount(expected.Count);
        results.Should().BeEquivalentTo(expected);

        expression = p => p.DeletedOn == null;
        expression = expression.And(p => p.FirstName.StartsWith(firstNamePrefix));
        expression = expression.And(p => p.LastName == person.LastName);
        results = queryable.Where(expression).ToList();
        results.Should().HaveCount(1);
        results.FirstOrDefault().Should().BeEquivalentTo(person);


        expression = p => p.DeletedOn == null;
        expression = expression.And(p => p.FirstName.StartsWith(firstNamePrefix));
        expression = expression.And(p => p.LastName == person.LastName);
        expression = expression.And(p => p.FirstName.StartsWith(firstNamePrefix));
        expression = expression.And(p => p.LastName == person.LastName);
        results = queryable.Where(expression).ToList();
        results.Should().HaveCount(1);
        results.FirstOrDefault().Should().BeEquivalentTo(person);
    }
}
