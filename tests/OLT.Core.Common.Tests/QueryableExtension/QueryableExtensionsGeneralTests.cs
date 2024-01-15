using FluentAssertions;
using System.Linq;
using OLT.Core.Common.Tests.Assets;
using Xunit;
using OLT.Core.Common.Tests.Assets.Models;

namespace OLT.Core.Common.Tests.QueryableExtension;

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

        var nonDeletedQueryable = list.AsQueryable().NonDeletedQueryable();
        nonDeletedQueryable.Should().BeEquivalentTo(expected);

        var personEntities = PersonEntity.FakerList(Faker.RandomNumber.Next(1, 29));
        var resultQuery = personEntities.AsQueryable().NonDeletedQueryable();
        resultQuery.Should().BeEquivalentTo(personEntities);

    }


}