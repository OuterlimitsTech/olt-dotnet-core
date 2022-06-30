using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.Core;
using OLT.Extensions.Caching.Tests.Assets;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.Caching.Tests
{
    public class RedisCacheGeneral
    {
        private readonly CacheConfiguration _config;

        public RedisCacheGeneral(IOptions<CacheConfiguration> options)
        {
            _config = options.Value;
        }


        [Fact]
        public void NewtonsoftCacheSerializerTests()
        {
            var model = TestHelper.CreateModel();
            var serializer = new OltNewtonsoftCacheSerializer();
            var result = serializer.Serialize(model);
            Assert.NotNull(result);
            var clone = serializer.Deserialize<OltPersonName>(result);
            clone.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void ExtensionExceptionTests()
        {
            var services = new ServiceCollection();
            string connectionString = null;

            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(null, TimeSpan.FromSeconds(15), ""));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), connectionString));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), ""));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), " "));

            ConfigurationOptions options = null;
            Assert.Throws<ArgumentNullException>("options", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), options));

            try
            {
                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), "localhost:6379");

                options = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { "localhost", 6379 },
                    },
                    ClientName = "test-app-2",
                };

                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), options);
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
            var provider = TestHelper.BuildRedisProvider(_config, TimeSpan.FromSeconds(2));
            var cacheService = provider.GetRequiredService<IOltCacheService>();
            

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model), TimeSpan.FromMilliseconds(1)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model), null).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));

            Assert.False(new ManualResetEvent(false).WaitOne(2500));
        }


        [Fact]
        public async Task TimeoutAsyncTests()
        {
            var provider = TestHelper.BuildRedisProvider(_config, TimeSpan.FromSeconds(2));
            var cacheService = provider.GetRequiredService<IOltCacheService>();
            

            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), TimeSpan.FromMilliseconds(1))).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), null)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(500));
            

            Assert.False(new ManualResetEvent(false).WaitOne(2500));
            
        }
    }
}