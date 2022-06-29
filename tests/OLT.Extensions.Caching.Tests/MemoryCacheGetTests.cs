using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.Caching.Tests
{

    public class MemoryCacheGet
    {

        [Fact]
        public async Task AsyncTests()
        {
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var services = new ServiceCollection();

            services.AddOltCacheMemory(TimeSpan.FromSeconds(15));
            var provider = services.BuildServiceProvider();

            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();


            await Assert.ThrowsAsync<ArgumentNullException>(async () => await oltMemoryCache.GetAsync(null, async () => await TestHelper.FakeAsync(model)));
            await Assert.ThrowsAsync<ArgumentException>(async () => await oltMemoryCache.GetAsync("", async () => await TestHelper.FakeAsync(model)));

            var result = await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model));

            result.Should().BeEquivalentTo(model);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


            //does it create a new entry? it should not
            (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CreateModel()))).Should().BeEquivalentTo(model);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


            oltMemoryCache.Remove(cacheKey);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();

        }

        [Fact]
        public void NonAsyncTest()
        {
            var model = TestHelper.CreateModel();
            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var services = new ServiceCollection();

            services.AddOltCacheMemory(TimeSpan.FromSeconds(15));
            var provider = services.BuildServiceProvider();


            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();

            Assert.Throws<ArgumentNullException>(() => oltMemoryCache.Get(null, () => TestHelper.CloneModel(model)));
            Assert.Throws<ArgumentException>(() => oltMemoryCache.Get("", () => TestHelper.CloneModel(model)));

            oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);


            //does it create a new entry? it should not
            oltMemoryCache.Get(cacheKey, () => TestHelper.CreateModel()).Should().BeEquivalentTo(model);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);

            oltMemoryCache.Remove(cacheKey);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        }


    }
}