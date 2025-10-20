using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltGeneralQueryableExtensionsTests
{

    public class TestEntity : IOltEntityDeletable
    {
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }

    [Fact]
    public void NonDeletedQueryable_ShouldReturnOnlyNonDeletedEntities()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new TestEntity { DeletedOn = null },
            new TestEntity { DeletedOn = DateTimeOffset.Now }
        }.AsQueryable();

        // Act
        var result = entities.NonDeletedQueryable();

        // Assert
        Assert.Single(result);
        Assert.Null(result.First().DeletedOn);
    }

    [Fact]
    public void NonDeletedQueryable_Random()
    {
        var entities = FakerList(Faker.RandomNumber.Next(5, 9), false);
        entities.AddRange(FakerList(Faker.RandomNumber.Next(1, 9), true));
        entities.AddRange(FakerList(Faker.RandomNumber.Next(5, 9), false));
        entities.AddRange(FakerList(Faker.RandomNumber.Next(1, 9), true));

        var randomized = RandomizeList(entities);
        var expected = randomized.Where(p => p.DeletedOn == null).ToList();

        var nonDeletedQueryable = randomized.AsQueryable().NonDeletedQueryable();
        nonDeletedQueryable.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void NonDeletedQueryable_NoDeleted()
    {
        var entities = FakerList(Faker.RandomNumber.Next(1, 29));
        var resultQuery = entities.AsQueryable().NonDeletedQueryable();
        resultQuery.Should().BeEquivalentTo(entities);

    }

    public static List<TestEntity> FakerList(int number, bool deleted = false)
    {
        var list = new List<TestEntity>();
        for (int i = 0; i < number; i++)
        {
            var item = new TestEntity();

            if (deleted)
            {
                item.DeletedBy = Faker.Name.FullName();
                item.DeletedOn = DateTimeOffset.Now;
            }

            list.Add(item);
        }
        return list;
    }

    public static List<TestEntity> RandomizeList(List<TestEntity> expected, int startingId = 1000, int minMix = 0, int maxMix = 6)
    {
        var randomized = expected.OrderBy(x => Guid.NewGuid()).ToList();
        var list = new List<TestEntity>();
        for (int i = 0; i < randomized.Count; i++)
        {
            var rangeList = FakerList(Faker.RandomNumber.Next(minMix, maxMix));
            rangeList.ForEach(item =>
            {
                //item.Id = startingId;
                startingId++;
                list.Add(item);
            });

            //randomized[i].Id = startingId;
            startingId++;
            list.Add(randomized[i]);
        }
        return list;
    }
}
