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
        public void ExtensionExceptionTests()
        {
            var services = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("services", () => OltMemoryCacheServiceCollectionExtensions.AddOltAddMemoryCache(null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15)));
            Assert.Throws<ArgumentNullException>("services", () => OltMemoryCacheServiceCollectionExtensions.AddOltAddMemoryCache(null, opt => new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(15))));
            Assert.Throws<ArgumentNullException>("setupAction", () => OltMemoryCacheServiceCollectionExtensions.AddOltAddMemoryCache(services, null));

            try
            {
                OltMemoryCacheServiceCollectionExtensions.AddOltAddMemoryCache(services, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15));
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
            var provider = TestHelper.BuildProvider(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
            var oltMemoryCache = provider.GetRequiredService<IOltMemoryCache>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model), TimeSpan.FromMilliseconds(1), null).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model), null, TimeSpan.FromMilliseconds(1)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        }


        [Fact]
        public async Task TimeoutAsyncTests()
        {
            var provider = TestHelper.BuildProvider(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
            var oltMemoryCache = provider.GetRequiredService<IOltMemoryCache>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), TimeSpan.FromMilliseconds(1), null)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await oltMemoryCache.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), null, TimeSpan.FromMilliseconds(1))).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            memoryCache.Get<OltPersonName>(cacheKey).Should().BeNull();
        }


    }
}