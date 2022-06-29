using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.Core;
using OLT.Extensions.Caching.Tests.Assets;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.Caching.Tests
{
    public class RedisCacheGet
    {
        private readonly CacheConfiguration _config;

        public RedisCacheGet(IOptions<CacheConfiguration> options)
        {
            _config = options.Value;
        }

        [Fact]
        public async Task AsyncTests()
        {
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var services = new ServiceCollection();

            services.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(TimeSpan.FromSeconds(15), _config.RedisCacheConnectionString);
            var provider = services.BuildServiceProvider();

            //var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var cacheService = provider.GetRequiredService<IOltCacheService>();


            await Assert.ThrowsAsync<ArgumentNullException>(async () => await cacheService.GetAsync(null, async () => await TestHelper.FakeAsync(model)));
            await Assert.ThrowsAsync<ArgumentException>(async () => await cacheService.GetAsync("", async () => await TestHelper.FakeAsync(model)));

            var result = await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model));

            result.Should().BeEquivalentTo(model);
            //memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


            //does it create a new entry? it should not
            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CreateModel()))).Should().BeEquivalentTo(model);
            //memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


            //cacheService.Remove(cacheKey);
            //memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();

        }

        //[Fact]
        //public void NonAsyncTest()
        //{
        //    var model = TestHelper.CreateModel();
        //    var cacheKey = $"cache-person-{Guid.NewGuid()}";
        //    var services = new ServiceCollection();

        //    services.AddOltCacheMemory(TimeSpan.FromSeconds(15));
        //    var provider = services.BuildServiceProvider();


        //    var memoryCache = provider.GetRequiredService<IMemoryCache>();
        //    var cacheService = provider.GetRequiredService<IOltCacheService>();

        //    Assert.Throws<ArgumentNullException>(() => cacheService.Get(null, () => TestHelper.CloneModel(model)));
        //    Assert.Throws<ArgumentException>(() => cacheService.Get("", () => TestHelper.CloneModel(model)));

        //    cacheService.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);
        //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


        //    //does it create a new entry? it should not
        //    cacheService.Get(cacheKey, () => TestHelper.CreateModel()).Should().BeEquivalentTo(model);
        //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);

        //    cacheService.Remove(cacheKey);
        //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        //}


    }
}