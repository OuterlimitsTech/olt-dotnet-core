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
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>(true));
                Assert.Null(adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject1, AdapterObject3>(true));


                Assert.Null(adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject2, AdapterObject3>(false));
                Assert.NotNull(adapterResolver.GetAdapter<AdapterObject2, AdapterObject3>(true));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject2, AdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(false));                ;
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<AdapterObject3, AdapterObject2>(true));               
                
            }
        }

        //[Fact]
        //public void GetAdapterNameTests()
        //{
        //    using (var provider = BuildProvider())
        //    {
        //        var adapterResolver = provider.GetService<IOltAdapterResolver>();
        //        Assert.Equal($"{typeof(AdapterObject1).FullName}->{typeof(AdapterObject2).FullName}", adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);
        //        Assert.Equal(OltAdapterExtensions.BuildAdapterName<AdapterObject1, AdapterObject2>(), adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);
        //        Assert.NotEqual($"{typeof(AdapterObject2)}->{typeof(AdapterObject1).FullName}", adapterResolver.GetAdapter<AdapterObject1, AdapterObject2>().Name);


        //        var beforeMap = new OltBeforeMapOrderBy<AdapterObject1, AdapterObject2>(p => p.OrderBy(t => t.FirstName));
        //        Assert.Equal($"{typeof(AdapterObject1).FullName}->{typeof(AdapterObject2).FullName}_BeforeMap", beforeMap.Name);
        //        Assert.Equal(OltAdapterExtensions.BuildBeforeMapName<AdapterObject1, AdapterObject2>(), beforeMap.Name);


        //        var afterMap = new OltAfterMapOrderBy<AdapterObject1, AdapterObject2>(p => p.OrderBy(t => t.Name.First));
        //        Assert.Equal($"{typeof(AdapterObject1).FullName}->{typeof(AdapterObject2).FullName}_AfterMap", afterMap.Name);
        //        Assert.Equal(OltAdapterExtensions.BuildAfterMapName<AdapterObject1, AdapterObject2>(), afterMap.Name);

        //    }
        //}

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
                Assert.False(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject1, AdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<AdapterObject2, AdapterObject3>());

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
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject1, AdapterObject2>(AdapterObject1.FakerList(3).AsQueryable()));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<AdapterObject2, AdapterObject3>(AdapterObject2.FakerList(3).AsQueryable()));
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




    }


}
