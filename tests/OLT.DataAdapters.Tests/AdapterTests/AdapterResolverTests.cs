using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.Tests.AdapterTests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {


        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>(true));
                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject3>(true));


                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject1>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject3>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject3>(true));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject3, BasicAdapterObject2>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject3, BasicAdapterObject2>(true));               
                
            }
        }

        [Fact]
        public void GetAdapterNameTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Equal($"{typeof(BasicAdapterObject1).FullName}->{typeof(BasicAdapterObject2).FullName}", adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>().Name);
                Assert.Equal(OltAdapterExtensions.BuildAdapterName<BasicAdapterObject1, BasicAdapterObject2>(), adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>().Name);
                Assert.NotEqual($"{typeof(BasicAdapterObject2)}->{typeof(BasicAdapterObject1).FullName}", adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>().Name);
            }
        }

        [Fact]
        public void CanMapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMap<BasicAdapterObject1, BasicAdapterObject2>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject2, BasicAdapterObject1>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject2, BasicAdapterObject3>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject3, BasicAdapterObject2>());

                Assert.False(adapterResolver.CanMap<BasicAdapterObject1, BasicAdapterObject3>());
                Assert.False(adapterResolver.CanMap<BasicAdapterObject3, BasicAdapterObject1>());

            }
        }

        [Fact]
        public void CanProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject1, BasicAdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject1, BasicAdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject2, BasicAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject2, BasicAdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject3, BasicAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject3, BasicAdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<BasicAdapterObject1, BasicAdapterObject2>(BasicAdapterObject1.FakerList(3).AsQueryable()));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<BasicAdapterObject2, BasicAdapterObject3>(BasicAdapterObject2.FakerList(3).AsQueryable()));
            }
        }

        [Fact]
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1 = BasicAdapterObject1.FakerData();
                
                var obj2Result = adapterResolver.Map<BasicAdapterObject1, BasicAdapterObject2>(obj1, new BasicAdapterObject2());
                Assert.Equal(obj1.FirstName, obj2Result.Name.First);
                Assert.Equal(obj1.LastName, obj2Result.Name.Last);
                adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject1>(obj2Result, new BasicAdapterObject1()).Should().BeEquivalentTo(obj1);


                var obj3 = BasicAdapterObject3.FakerData();
                obj2Result = adapterResolver.Map<BasicAdapterObject3, BasicAdapterObject2>(obj3, new BasicAdapterObject2());
                Assert.Equal(obj3.First, obj2Result.Name.First);
                Assert.Equal(obj3.Last, obj2Result.Name.Last);
                adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject3>(obj2Result, new BasicAdapterObject3()).Should().BeEquivalentTo(obj3);
            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetService<IOltAdapterResolver>();
                var obj1Values = BasicAdapterObject1.FakerList(3);
                var obj2Result = adapterResolver.Map<BasicAdapterObject1, BasicAdapterObject2>(obj1Values);
                obj2Result.Should().HaveCount(obj1Values.Count);
                obj2Result.Select(s => s.Name.First).Should().BeEquivalentTo(obj1Values[0].FirstName, obj1Values[1].FirstName, obj1Values[2].FirstName);
                obj2Result.Select(s => s.Name.Last).Should().BeEquivalentTo(obj1Values[0].LastName, obj1Values[1].LastName, obj1Values[2].LastName);
                adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject1>(obj2Result).Should().BeEquivalentTo(obj1Values);


                var obj3Values = BasicAdapterObject3.FakerList(3);
                obj2Result = adapterResolver.Map<BasicAdapterObject3, BasicAdapterObject2>(obj3Values);
                obj2Result.Should().HaveCount(obj3Values.Count);
                obj2Result.Select(s => s.Name.First).Should().BeEquivalentTo(obj3Values[0].First, obj3Values[1].First, obj3Values[2].First);
                obj2Result.Select(s => s.Name.Last).Should().BeEquivalentTo(obj3Values[0].Last, obj3Values[1].Last, obj3Values[2].Last);

                adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject3>(obj2Result).Should().BeEquivalentTo(obj3Values);

            }
        }




    }


}
