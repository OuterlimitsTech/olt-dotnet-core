using FluentAssertions;
using OLT.Core;
using OLT.Core.Services.Tests.Assets;
using OLT.Core.Services.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace OLT.Core.Services.Tests
{
    public class QueryableExtensionsGeneralTests
    {
        [Fact]
        public void NonDeletedQueryableTests()
        {
            var entityPersons = EntityPersonModel.FakerList(Faker.RandomNumber.Next(5, 9), false);
            entityPersons.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(1, 9), true));
            entityPersons.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(5, 9), false));
            entityPersons.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(1, 9), true));

            var list = TestHelper.BuildTestList(entityPersons);
            var expected = list.Where(p => p.DeletedOn == null).ToList();

            var nonDeletedQueryable = OltGeneralQueryableExtensions.NonDeletedQueryable(list.AsQueryable());
            nonDeletedQueryable.Should().BeEquivalentTo(expected);

            var personEntities = PersonEntity.FakerList(Faker.RandomNumber.Next(1, 29));
            var resultQuery = OltGeneralQueryableExtensions.NonDeletedQueryable(personEntities.AsQueryable());
            resultQuery.Should().BeEquivalentTo(personEntities);

        }


    }
}
