using FluentAssertions;
using OLT.Core;
using OLT.Core.Services.Tests.Assets;
using OLT.Core.Services.Tests.Assets.Models;
using System;
using System.Linq;
using OLT.Core.Common.Tests.Assets;
using Xunit;

namespace OLT.Core.Services.Tests;

public class SearcherTests
{

    [Fact]
    public void SearcherGetById()
    {            
        var expected = EntityPersonModel.FakerData();
        var list = TestHelper.BuildTestList(expected);
        var queryable = list.AsQueryable();

        Assert.False(new OltSearcherGetById<EntityPersonModel>(expected.Id).IncludeDeleted);
        Assert.False(new OltSearcherGetById<EntityPersonModel>(expected.Id, false).IncludeDeleted);
        Assert.True(new OltSearcherGetById<EntityPersonModel>(expected.Id, true).IncludeDeleted);
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
        var list = TestHelper.BuildTestList(expected);
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
        var firstName = nameof(IncludeDeleted);
        Assert.False(new PersonFirstNameStartsWithSearcher(firstName).IncludeDeleted);
        Assert.True(new PersonFirstNameStartsWithSearcher(firstName, true).IncludeDeleted);
        Assert.False(new PersonFirstNameStartsWithSearcher(firstName, false).IncludeDeleted);
    }



   
}