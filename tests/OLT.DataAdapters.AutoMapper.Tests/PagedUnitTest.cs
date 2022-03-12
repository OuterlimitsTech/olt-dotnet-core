using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public class PagedUnitTest : BaseAdpaterTests
    {
        private readonly Func<IQueryable<AdapterObject3>, IQueryable<AdapterObject3>> _defaultOrder = entity => entity.OrderBy(p => p.Last).ThenBy(p => p.First).ThenBy(p => p.ObjectId);

        [Fact]
        public void DisposableTests()
        {
            using (var adapter1 = new AdapterObject4PagedMap())
            {
                Assert.False(adapter1.IsDisposed());
            }

            var adapter = new AdapterObject4PagedMap();
            Assert.NotNull(adapter);
            Assert.False(adapter.IsDisposed());
            adapter.Dispose();
            Assert.True(adapter.IsDisposed());
        }

        [Fact]
        public void GeneralTests()
        {
            //Assert.Throws<NotImplementedException>(() => new AdapterObject5PagedMap());  //Constructor -> CreateMap -> BuildMap
            Assert.Equal(OltAdapterExtensions.BuildAdapterName<AdapterObject2, AdapterObject4>(), new AdapterObject4PagedMap().Name);
        }

        //[Fact]
        //public void PagedTests()
        //{

        //    var @params = new OltPagingParams
        //    {
        //        Page = Faker.RandomNumber.Next(1, 4),
        //        Size = Faker.RandomNumber.Next(10, 20)
        //    };

        //    //var paged = OltPagedExtensions.ToPaged(queryable, @params, _defaultOrder);

        //    //Assert.Equal(@params.Page, paged.Page);
        //    //Assert.Equal(@params.Size, paged.Size);
        //    //Assert.Equal(list.Count, paged.Count);
        //    //Assert.Equal(@params.Size, paged.Data.Count());

        //    //var expected = ExpectedPage(_defaultOrder(list.AsQueryable()).ToList(), @params);
        //    //paged.Data.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());

        //}

        //[Fact]
        //public void MapTests()
        //{

        //    using (var provider = BuildProvider())
        //    {
        //        var adapterResolver = provider.GetService<IOltAdapterResolver>();
        //        var list = AdapterObject2.FakerList(47);
        //        var queryable = list.AsQueryable();



        //        var obj2Result = adapterResolver.ProjectTo<AdapterObject2, AdapterObject4>(queryable);
        //        Assert.Equal(obj1.FirstName, obj2Result.Name.First);
        //        Assert.Equal(obj1.LastName, obj2Result.Name.Last);
        //        adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result, new AdapterObject1()).Should().BeEquivalentTo(obj1);


        //        var obj3 = AdapterObject3.FakerData();
        //        obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3, new AdapterObject2());
        //        Assert.Equal(obj3.First, obj2Result.Name.First);
        //        Assert.Equal(obj3.Last, obj2Result.Name.Last);
        //        adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result, new AdapterObject3()).Should().BeEquivalentTo(obj3);
        //    }
        //}
    }
}