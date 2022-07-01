using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.Core;
using OLT.Extensions.Caching.Tests.Assets;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
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
            Func<Task<OltPersonName>> nullFactory = null;
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var provider = TestHelper.BuildRedisProvider(_config, TimeSpan.FromSeconds(30), "async-tests");

            var cacheService = provider.GetRequiredService<IOltCacheService>();
            

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await cacheService.GetAsync(null, async () => await TestHelper.FakeAsync(model)));
            await Assert.ThrowsAsync<ArgumentException>(async () => await cacheService.GetAsync("", async () => await TestHelper.FakeAsync(model)));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await cacheService.GetAsync(cacheKey, nullFactory));

            var result = await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model));

            result.Should().BeEquivalentTo(model);

            //does it create a new entry? it should not
            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CreateModel()))).Should().BeEquivalentTo(model);
            
            await cacheService.RemoveAsync(cacheKey);

            var model2 = TestHelper.CreateModel();
            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model2))).Should().NotBeEquivalentTo(model);  

            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model))).Should().BeEquivalentTo(model2);
        }

        [Fact]
        public void NonAsyncTest()
        {
            Func<OltPersonName> nullFactory = null;
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var provider = TestHelper.BuildRedisProvider(_config, TimeSpan.FromSeconds(30), "non-async-tests");

            var cacheService = provider.GetRequiredService<IOltCacheService>();

            Assert.Throws<ArgumentNullException>(() => cacheService.Get(null, () => TestHelper.CloneModel(model)));
            Assert.Throws<ArgumentException>(() => cacheService.Get("", () => TestHelper.CloneModel(model)));
            Assert.Throws<ArgumentNullException>(() => cacheService.Get(cacheKey, nullFactory));
            

            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);

            //does it create a new entry? it should not
            cacheService.Get(cacheKey, () => TestHelper.CreateModel()).Should().BeEquivalentTo(model);

            cacheService.Remove(cacheKey);

            var model2 = TestHelper.CreateModel();            
            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model2)).Should().NotBeEquivalentTo(model);
            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model2);
        }

        [Fact]
        public async Task AsyncConfigTests()
        {
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var provider = TestHelper.BuildRedisProvider(_config, TimeSpan.FromSeconds(30), "async-config-tests");           
            
            var cacheService = provider.GetRequiredService<IOltCacheService>();

            var result = await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model));

            result.Should().BeEquivalentTo(model);

            //does it create a new entry? it should not
            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CreateModel()))).Should().BeEquivalentTo(model);

            cacheService.Remove(cacheKey);

        }
    }
}