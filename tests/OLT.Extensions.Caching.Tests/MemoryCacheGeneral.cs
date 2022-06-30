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

    public class MemoryCacheGeneral
    {

        [Fact]
        public void CacheOptionsTests()
        {
            var timespan = TimeSpan.FromSeconds(Faker.RandomNumber.Next());
            var options = new OltCacheOptions();
            options.DefaultAbsoluteExpiration = timespan;
            Assert.Equal(timespan, options.DefaultAbsoluteExpiration);
            options.Value.Should().BeEquivalentTo(options);
        }

        [Fact]
        public void ExtensionExceptionTests()
        {
            var services = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("services", () => OltMemoryCacheServiceCollectionExtensions.AddOltCacheMemory(null, TimeSpan.FromSeconds(15)));

            try
            {
                OltMemoryCacheServiceCollectionExtensions.AddOltCacheMemory(services, TimeSpan.FromSeconds(15));
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void TimeoutTests()
        {
            var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model), TimeSpan.FromMilliseconds(1)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model), null).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().NotBeNull();

            Assert.False(new ManualResetEvent(false).WaitOne(2500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        }


        [Fact]
        public async Task TimeoutAsyncTests()
        {
            var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
            var oltMemoryCache = provider.GetRequiredService<IOltCacheService>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), TimeSpan.FromMilliseconds(1))).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), null)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().NotBeNull();

            Assert.False(new ManualResetEvent(false).WaitOne(2500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        }


    }
}