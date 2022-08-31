using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Collections.Generic;
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

            (await oltMemoryCache.ExistsAsync(cacheKey)).Should().BeTrue();
            (await oltMemoryCache.ExistsAsync(Faker.Lorem.GetFirstWord())).Should().BeFalse();

            await oltMemoryCache.RemoveAsync(cacheKey);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
            (await oltMemoryCache.ExistsAsync(cacheKey)).Should().BeFalse();

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

            oltMemoryCache.Exists(cacheKey).Should().BeTrue();
            oltMemoryCache.Exists(Faker.Lorem.GetFirstWord()).Should().BeFalse();
            
            oltMemoryCache.Remove(cacheKey);
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
            oltMemoryCache.Exists(cacheKey).Should().BeFalse();            
            
        }

        [Fact]
        public void FlushTests()
        {
            
            var cacheKeys = new Dictionary<string, OltPersonName>();
            var services = new ServiceCollection();

            services.AddOltCacheMemory(TimeSpan.FromMinutes(2));
            var provider = services.BuildServiceProvider();

            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();

            for(var idx = 0; idx < 10; idx++)
            {
                var model = TestHelper.CreateModel();
                var cacheKey = $"cache-person-{Guid.NewGuid()}";
                cacheKeys.Add(cacheKey, model);
                oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);
            }

            foreach(var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(expected);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(expected);
            }

            oltMemoryCache.Flush();

            foreach (var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(newModel);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(newModel);
            }
        }

        [Fact]
        public async Task FlushAsyncTests()
        {

            var cacheKeys = new Dictionary<string, OltPersonName>();
            var services = new ServiceCollection();

            services.AddOltCacheMemory(TimeSpan.FromMinutes(2));
            var provider = services.BuildServiceProvider();

            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();

            for (var idx = 0; idx < 10; idx++)
            {
                var model = TestHelper.CreateModel();
                var cacheKey = $"cache-person-{Guid.NewGuid()}";
                cacheKeys.Add(cacheKey, model);
                (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model))).Should().BeEquivalentTo(model);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);
            }

            foreach (var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(expected);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(expected);
            }

            await oltMemoryCache.FlushAsync();

            foreach (var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(newModel);
                memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(newModel);
            }
        }
    }
}