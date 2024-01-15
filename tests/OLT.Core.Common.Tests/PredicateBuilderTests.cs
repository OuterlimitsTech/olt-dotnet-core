using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OLT.Core.Common.Tests.Assets;
using Xunit;
using OLT.Core.Common.Tests.Assets.Models;

namespace OLT.Core.Common.Tests;

public class PredicateBuilderTests
{
    [Fact]
    public void OrTests()
    {
        var firstNames = new List<string>
        {
            $"{nameof(OrTests)}{Guid.NewGuid()}",
            $"{nameof(OrTests)}{Guid.NewGuid()}",
            $"{nameof(OrTests)}{Guid.NewGuid()}",
            $"{nameof(OrTests)}{Guid.NewGuid()}"
        };

        var expected = EntityPersonModel.FakerList(4);
        for (var i = 0; i < firstNames.Count; i++)
        {
            expected[i].FirstName = firstNames[i];
        }
        var list = TestHelper.BuildTestList(expected, 1000, 7, 12);
        var queryable = list.AsQueryable();

        Expression<Func<EntityPersonModel, bool>> expression = null;

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
    public void AndTests()
    {
        var firstNamePrefix = nameof(AndTests);
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