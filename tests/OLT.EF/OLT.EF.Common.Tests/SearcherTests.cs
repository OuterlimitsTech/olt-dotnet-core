﻿using FluentAssertions;
using OLT.Core;
using OLT.EF.Common.Tests.Assets;
using OLT.EF.Common.Tests.Assets.Models;
using System;
using System.Linq;
using Xunit;

namespace OLT.EF.Common.Tests
{    
    public class SearcherTests
    {

        [Fact]
        public void SearcherGetById()
        {            
            var expected = EntityPersonModel.FakerData();
            var list = TestHelper.BuildTestList(expected);
            var queryable = list.AsQueryable();

            Assert.True(new OltSearcherGetById<EntityPersonModel>(expected.Id).IncludeDeleted);
            Assert.False(new OltSearcherGetById<EntityPersonModel>(expected.Id, false).IncludeDeleted);
            Assert.Equal(expected.Id, new OltSearcherGetById<EntityPersonModel>(expected.Id).Id);

            var searcher = new OltSearcherGetById<EntityPersonModel>(expected.Id);
            var results = searcher.BuildQueryable(queryable).ToList();
            results.Should().HaveCount(1);
            results.FirstOrDefault().Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void SearcherGetAll()
        {
            var expected = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            var queryable = expected.AsQueryable();

            Assert.False(new OltSearcherGetAll<EntityPersonModel>().IncludeDeleted);
            Assert.True(new OltSearcherGetAll<EntityPersonModel>(true).IncludeDeleted);

            var searcher = new OltSearcherGetAll<EntityPersonModel>();
            var results = searcher.BuildQueryable(queryable).ToList();
            results.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SearcherGetByUid()
        {
            var expected = EntityPersonModel.FakerData();
            var list = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            var queryable = list.AsQueryable();

            Assert.True(new OltSearcherGetByUid<EntityPersonModel>(expected.UniqueId).IncludeDeleted);
            Assert.False(new OltSearcherGetByUid<EntityPersonModel>(expected.UniqueId, false).IncludeDeleted);
            Assert.Equal(expected.UniqueId, new OltSearcherGetByUid<EntityPersonModel>(expected.UniqueId).Uid);

            var searcher = new OltSearcherGetByUid<EntityPersonModel>(expected.UniqueId);
            var results = searcher.BuildQueryable(queryable).ToList();
            results.Should().HaveCount(1);
            results.FirstOrDefault().Should().BeEquivalentTo(expected);

        }


        [Fact]
        public void IncludeDeleted()
        {
            var firstName = Faker.Name.First();
            Assert.False(new PersonFirstNameStartsWithSearcher(firstName).IncludeDeleted);
            Assert.True(new PersonFirstNameStartsWithSearcher(firstName, true).IncludeDeleted);
            Assert.False(new PersonFirstNameStartsWithSearcher(firstName, false).IncludeDeleted);
        }



        [Fact]
        public void WhereExtensionTests()
        {
            var firstName = Faker.Name.First();
            var searcher = new PersonFirstNameStartsWithSearcher(firstName);

            var expected = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25), true));
            expected.SetFirstNameStartsWith(firstName);

            var list = TestHelper.BuildTestList(expected);
            var queryable = list.AsQueryable();


            Assert.NotEmpty(list.Where(p => p.DeletedOn == null));
            Assert.NotEmpty(list.Where(p => p.DeletedOn != null));
            

            var results = queryable.Where(searcher).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher = new PersonFirstNameStartsWithSearcher(firstName, true);
            
            results = queryable.Where(searcher).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher = new PersonFirstNameStartsWithSearcher(firstName, false);
            Assert.False(searcher.IncludeDeleted);
            results = queryable.Where(searcher).ToList();
            results.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhereExtensionArrayTests()
        {
            var firstName = Faker.Name.First();
            var lastName = Faker.Name.First();
            var searcher1 = new PersonLastNameStartsWithSearcher(lastName);
            var searcher2 = new PersonFirstNameStartsWithSearcher(firstName);

            var expected = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25), true));
            expected.SetFirstNameStartsWith(firstName);
            expected.SetLastNameStartsWith(lastName);

            var list = TestHelper.BuildTestList(expected);
            var queryable = list.AsQueryable();

            Assert.NotEmpty(list.Where(p => p.DeletedOn == null));
            Assert.NotEmpty(list.Where(p => p.DeletedOn != null));

            var results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher2 = new PersonFirstNameStartsWithSearcher(firstName, true);
            results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);


            searcher2 = new PersonFirstNameStartsWithSearcher(firstName, false);
            results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);
        }
    }
}
