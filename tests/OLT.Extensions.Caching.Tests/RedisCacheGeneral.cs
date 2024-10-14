using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.Core;
using OLT.Extensions.Caching.Tests.Assets;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Testcontainers.Redis;
using Xunit;

namespace OLT.Extensions.Caching.Tests
{
    [Collection("Redis")]
    public class RedisCacheGeneral : IAsyncLifetime
    {
        private readonly RedisContainer _redis = new RedisBuilder().Build();

        public async Task InitializeAsync()
        {
            await _redis.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _redis.StopAsync();
        }

        //private readonly CacheConfiguration _config;

        //public RedisCacheGeneral(IOptions<CacheConfiguration> options)
        //{
        //    _config = options.Value;
        //}


        //[Fact]
        //public void NewtonsoftCacheSerializerTests()
        //{
        //    var model = TestHelper.CreateModel();
        //    var serializer = new OltNewtonsoftCacheSerializer();
        //    var result = serializer.Serialize(model);
        //    Assert.NotNull(result);
        //    var clone = serializer.Deserialize<OltPersonName>(result);
        //    clone.Should().BeEquivalentTo(model);
        //}

        [Fact]
        public void ExtensionExceptionTests()
        {
            var services = new ServiceCollection();
            string connectionString = null;
            string cacheKeyPrefix = "test-app";

            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(null, TimeSpan.FromSeconds(15), cacheKeyPrefix, "abc-123"));
            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(null, TimeSpan.FromSeconds(15), null, "abc-123"));
            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(null, TimeSpan.FromSeconds(15), "", "abc-123"));

            Assert.Throws<ArgumentNullException>("cacheKeyPrefix", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), "", "abc-123"));
            Assert.Throws<ArgumentNullException>("cacheKeyPrefix", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), " ", "abc-123"));
            Assert.Throws<ArgumentNullException>("cacheKeyPrefix", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), null, "abc-123"));

            Assert.Throws<ArgumentNullException>("connectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(null, TimeSpan.FromSeconds(15), cacheKeyPrefix, ""));
            Assert.Throws<ArgumentNullException>("connectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, connectionString));
            Assert.Throws<ArgumentNullException>("connectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, ""));
            Assert.Throws<ArgumentNullException>("connectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, " "));

            StackExchange.Redis.Extensions.Core.Configuration.RedisConfiguration options = null;
            Assert.Throws<ArgumentNullException>("redisConfiguration", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, options));

            var host = new List<RedisHost>();
            host.Add(new RedisHost { Host = "localhost", Port = 6379 });
            options = new StackExchange.Redis.Extensions.Core.Configuration.RedisConfiguration();
            options.Hosts = host.ToArray();

            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(null, TimeSpan.FromSeconds(15), cacheKeyPrefix, options));

            try
            {
                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, "localhost:6379");




                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis(services, TimeSpan.FromSeconds(15), cacheKeyPrefix, options);
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
            var provider = TestHelper.BuildRedisProvider(_redis, TimeSpan.FromSeconds(4), "TimeoutTests");
            var cacheService = provider.GetRequiredService<IOltCacheService>();


            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            var result = cacheService.Get(cacheKey, () => TestHelper.CloneModel(model), TimeSpan.FromSeconds(1)).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(1000));


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            cacheService.Get(cacheKey, () => TestHelper.CloneModel(model), null).Should().BeEquivalentTo(model);
            //Assert.False(new ManualResetEvent(false).WaitOne(1000));


        }


        [Fact]
        public async Task TimeoutAsyncTests()
        {
            var provider = TestHelper.BuildRedisProvider(_redis, TimeSpan.FromSeconds(2), "TimeoutAsyncTests");
            var cacheService = provider.GetRequiredService<IOltCacheService>();


            var cacheKey = $"cache-person-{Guid.NewGuid()}";
            var model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), TimeSpan.FromSeconds(1))).Should().BeEquivalentTo(model);
            Assert.False(new ManualResetEvent(false).WaitOne(1000));


            model = TestHelper.CreateModel(Guid.NewGuid().ToString());

            (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(TestHelper.CloneModel(model)), null)).Should().BeEquivalentTo(model);
            //Assert.False(new ManualResetEvent(false).WaitOne(500));



        }


    }
}