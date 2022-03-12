using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.DataAdapters.Tests.PagedAdapterTests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {


        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>(true));
                Assert.Null(adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(true));                
            }
        }

        [Fact]
        public void GetAdapterNameTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Equal($"{typeof(AdapterObject1).FullName}->{typeof(AdapterObject2).FullName}", adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);
                Assert.Equal(OltAdapterExtensions.BuildAdapterName<AdapterObject1, AdapterObject2>(), adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);
                Assert.NotEqual($"{typeof(AdapterObject2)}->{typeof(AdapterObject1).FullName}", adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);
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
                Assert.False(adapterResolver.CanMap<AdapterObject2, AdapterObject3>());
                Assert.False(adapterResolver.CanMap<AdapterObject3, AdapterObject2>());
                Assert.True(adapterResolver.CanMap<AdapterObject1, AdapterObject3>());
                Assert.True(adapterResolver.CanMap<AdapterObject3, AdapterObject1>());
            }
        }

        [Fact]
        [Obsolete("Legacy Paged Process")]
        public void CanMapPagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMapPaged<AdapterObject1, AdapterObject2>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject2, AdapterObject1>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject2, AdapterObject3>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject3, AdapterObject2>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject1, AdapterObject3>());
                Assert.False(adapterResolver.CanMapPaged<AdapterObject3, AdapterObject1>());
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

                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject3>());

                Assert.True(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();                
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject2, AdapterObject3>(AdapterObject2.FakerList(3).AsQueryable()));

                try
                {
                    adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(AdapterObject1.FakerList(3).AsQueryable());
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }
            }
        }

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
            }
        }

        [Fact]
        [Obsolete("Legacy Paged Process")]
        public void LegacyPagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(10);
                var queryable = obj1Values.AsQueryable();

                var expectedResults = obj1Values.Select(s => new AdapterObject2 {  Name = new OltPersonName {  First = s.FirstName, Last = s.LastName } });
                
                var pagingParams = new OltPagingParams { Page = 1, Size = 25 };
                var results = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(queryable, pagingParams);
                results.Data.Should().BeEquivalentTo(expectedResults.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First));

                
                results = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(queryable, pagingParams, orderBy => orderBy.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName));
                results.Data.Should().BeEquivalentTo(expectedResults.OrderByDescending(p => p.Name.Last).ThenByDescending(p => p.Name.First));
            }
        }

        [Fact]
        public void PagedTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj3Values = AdapterObject3.FakerList(10);
                var queryable = obj3Values.AsQueryable();

                var expectedResults = obj3Values.Select(s => new AdapterObject1 { FirstName = s.First, LastName = s.Last });

                var pagingParams = new OltPagingParams { Page = 1, Size = 25 };
                var results = adapterResolver.ProjectTo<AdapterObject3, AdapterObject1>(queryable).ToPaged(pagingParams);
                results.Data.Should().BeEquivalentTo(expectedResults.OrderBy(p => p.LastName).ThenBy(p => p.FirstName));

                //results = adapterResolver.ProjectTo<AdapterObject3, AdapterObject1>(queryable, orderBy => orderBy.OrderByDescending(p => p.Last).ThenByDescending(p => p.First));
                //results.Data.Should().BeEquivalentTo(expectedResults.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName));


                //results = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(queryable, pagingParams, orderBy => orderBy.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName));
                //results.Data.Should().BeEquivalentTo(expectedResults.OrderByDescending(p => p.Name.Last).ThenByDescending(p => p.Name.First));
            }
        }

    }


}
