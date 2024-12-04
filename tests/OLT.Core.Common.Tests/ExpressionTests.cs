using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using OLT.Core.Common.Tests.Assets.Models;

namespace OLT.Core.Common.Tests;

public class ExpressionTests
{
    [Fact]
    public void IntContains()
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


    [Fact]
    public void Int()
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

    [Fact]
    public void IntNullable()
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

    [Fact]
    public void StringContains()
    {
        var firstName = nameof(StringContains);
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

    [Fact]
    public void StringStartsWith()
    {
        var firstName = nameof(StringStartsWith);

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

    [Fact]
    public void StringEquals()
    {
        var firstName = nameof(StringEquals);

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