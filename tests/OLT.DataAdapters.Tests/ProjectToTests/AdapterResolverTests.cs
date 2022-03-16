using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.Tests.ProjectToTests
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


                Assert.Null(adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject2, AdapterObject3>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject2, AdapterObject3>(true));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(false)); ;
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(true));

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
                Assert.True(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject3, AdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1Values = AdapterObject1.FakerList(23);

                var obj2ResultQueryable = adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable());
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject2, AdapterObject1>(obj2ResultQueryable));

                var expected = obj1Values                    
                    .Select(s => new AdapterObject2
                    {
                        Name = new OltPersonName
                        {
                            First = s.FirstName,
                            Last = s.LastName,
                        }
                    }).ToList();

                obj2ResultQueryable.Should().BeEquivalentTo(expected.OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last), opt => opt.WithStrictOrdering());

                
                adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(obj1Values.AsQueryable(), configAction => { configAction.DisableBeforeMap = true; configAction.DisableAfterMap = true;  })
                    .Should()
                    .BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
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
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<AdapterObject1, AdapterObject2>(AdapterObject1.FakerData(), new AdapterObject2()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<AdapterObject2, AdapterObject3>(AdapterObject2.FakerData(), new AdapterObject3()));
            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Throws<AggregateException>(() => adapterResolver.Map<AdapterObject1, AdapterObject2>(AdapterObject1.FakerList(3)));
                Assert.Throws<AggregateException>(() => adapterResolver.Map<AdapterObject2, AdapterObject3>(AdapterObject2.FakerList(3)));
            }
        }
    }
}
