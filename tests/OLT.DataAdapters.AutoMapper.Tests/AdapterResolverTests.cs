using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {


        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Null(adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(true));


                Assert.Null(adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(false)); ;
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(true));

            }
        }


        [Fact]
        public void CanMapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMap<AdapterObject1, AdapterObject2>());
                Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject1>());
                Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject3>());
                Assert.True(adapterResolver.CanMap<AdapterObject3, AdapterObject2>());

                Assert.False(adapterResolver.CanMap<AdapterObject1, AdapterObject3>());
                Assert.False(adapterResolver.CanMap<AdapterObject3, AdapterObject1>());

            }
        }

        [Fact]
        public void CanProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject3>());

                Assert.True(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject2>());
            }
        }

        //[Fact]
        //public void ProjectToTests()
        //{
        //    using (var provider = BuildProvider())
        //    {
        //        var adapterResolver = provider.GetService<IOltAdapterResolver>();
        //        Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(AdapterObject1.FakerList(3).AsQueryable()));
        //        Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject2, AdapterObject3>(AdapterObject2.FakerList(3).AsQueryable()));
        //    }
        //}

        [Fact]
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1 = AdapterObject1.FakerData();

                var obj2Result = adapterResolver.Map<AdapterObject1, AdapterObject2>(obj1, new AdapterObject2());
                Assert.Equal(obj1.FirstName, obj2Result.Name.First);
                Assert.Equal(obj1.LastName, obj2Result.Name.Last);
                adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result, new AdapterObject1()).Should().BeEquivalentTo(obj1);


                var obj3 = AdapterObject3.FakerData();
                obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3, new AdapterObject2());
                Assert.Equal(obj3.First, obj2Result.Name.First);
                Assert.Equal(obj3.Last, obj2Result.Name.Last);
                adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result, new AdapterObject3()).Should().BeEquivalentTo(obj3);
            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(3);
                var obj2Result = adapterResolver.Map<AdapterObject1, AdapterObject2>(obj1Values);
                obj2Result.Should().HaveCount(obj1Values.Count);
                obj2Result.Select(s => s.Name.First).Should().BeEquivalentTo(obj1Values[0].FirstName, obj1Values[1].FirstName, obj1Values[2].FirstName);
                obj2Result.Select(s => s.Name.Last).Should().BeEquivalentTo(obj1Values[0].LastName, obj1Values[1].LastName, obj1Values[2].LastName);
                adapterResolver.Map<AdapterObject2, AdapterObject1>(obj2Result).Should().BeEquivalentTo(obj1Values);


                var obj3Values = AdapterObject3.FakerList(3);
                obj2Result = adapterResolver.Map<AdapterObject3, AdapterObject2>(obj3Values);
                obj2Result.Should().HaveCount(obj3Values.Count);
                obj2Result.Select(s => s.Name.First).Should().BeEquivalentTo(obj3Values[0].First, obj3Values[1].First, obj3Values[2].First);
                obj2Result.Select(s => s.Name.Last).Should().BeEquivalentTo(obj3Values[0].Last, obj3Values[1].Last, obj3Values[2].Last);

                adapterResolver.Map<AdapterObject2, AdapterObject3>(obj2Result).Should().BeEquivalentTo(obj3Values);

            }
        }

        [Fact]
        [Obsolete("Legacy Paged Process")]
        public void CanMapPagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.False(adapterResolver.CanMapPaged<AdapterObject1, AdapterObject2>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject2, AdapterObject3>());
                Assert.True(adapterResolver.CanMapPaged<AdapterObject2, AdapterObject4>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject4, AdapterObject2>());
            }
        }


        [Fact]
        [Obsolete("Legacy Paged Process")]
        public void LegacyPagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var pagingParams = new OltPagingParams { Page = 1, Size = 25 };

                var obj2Values = AdapterObject2.FakerList(10);
                var queryable = obj2Values.AsQueryable();

                var expectedResults = obj2Values.Select(s => new AdapterObject4 { ObjectId = s.ObjectId, Name = new OltPersonName { First = s.Name.First, Last = s.Name.Last } });

                var results = adapterResolver.ProjectTo<AdapterObject2, AdapterObject4>(queryable, pagingParams);
                results.Data.Should().BeEquivalentTo(expectedResults.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First).ThenBy(p => p.ObjectId));


                results = adapterResolver.ProjectTo<AdapterObject2, AdapterObject4>(queryable, pagingParams, orderBy => orderBy.OrderByDescending(p => p.Name.Last).ThenByDescending(p => p.Name.First).ThenBy(p => p.ObjectId));
                results.Data.Should().BeEquivalentTo(expectedResults.OrderByDescending(p => p.Name.Last).ThenByDescending(p => p.Name.First).ThenBy(p => p.ObjectId));


                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject2, AdapterObject3>(AdapterObject2.FakerList(10).AsQueryable(), pagingParams));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject3, AdapterObject5>(AdapterObject3.FakerList(10).AsQueryable(), pagingParams));
            }
        }



    }


}
