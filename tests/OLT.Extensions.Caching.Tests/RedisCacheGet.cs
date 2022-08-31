using Faker;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OLT.Core;
using OLT.Extensions.Caching.Tests.Assets;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
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

            (await cacheService.ExistsAsync(cacheKey)).Should().BeTrue();
            (await cacheService.ExistsAsync(Faker.Lorem.GetFirstWord())).Should().BeFalse();
            
            await cacheService.RemoveAsync(cacheKey);

            (await cacheService.ExistsAsync(cacheKey)).Should().BeFalse();

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

            cacheService.Exists(cacheKey).Should().BeTrue();
            cacheService.Exists(Faker.Lorem.GetFirstWord()).Should().BeFalse();

            cacheService.Remove(cacheKey);
            cacheService.Exists(cacheKey).Should().BeFalse();
            
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



        //[Fact]
        //public void FlushTests()
        //{

        //    var cacheKeys = new Dictionary<string, OltPersonName>();
        //    var services = new ServiceCollection();
        //    var config = new RedisConfiguration
        //    {
        //        ConnectionString = $"{_config.RedisCacheConnectionString};database=3"
        //    };

        //    var provider = TestHelper.BuildRedisProvider(config, TimeSpan.FromMinutes(2), "flush-tests");

        //    var cacheService = provider.GetRequiredService<IOltCacheService>();

        //    //for (var idx = 0; idx < 10; idx++)
        //    //{
        //    //    var model = TestHelper.CreateModel();
        //    //    var cacheKey = $"cache-person-{Guid.NewGuid()}";
        //    //    cacheKeys.Add(cacheKey, model);
        //    //    oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);
        //    //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(model);
        //    //}

        //    //foreach (var item in cacheKeys)
        //    //{
        //    //    var newModel = TestHelper.CreateModel();
        //    //    var cacheKey = item.Key;
        //    //    var expected = item.Value;
        //    //    oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(expected);
        //    //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(expected);
        //    //}

        //    //oltMemoryCache.Flush();

        //    //foreach (var item in cacheKeys)
        //    //{
        //    //    var newModel = TestHelper.CreateModel();
        //    //    var cacheKey = item.Key;
        //    //    var expected = item.Value;
        //    //    oltMemoryCache.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(newModel);
        //    //    memoryCache.Get<OltPersonName>(cacheKey).Should().BeEquivalentTo(newModel);
        //    //}
        //}

        [Fact]
        public async Task FlushAsyncTests()
        {

            var cacheKeys = new Dictionary<string, OltPersonName>();
            var services = new ServiceCollection();

            
            var config = new RedisConfiguration
            {
                //ConnectionString = _config.RedisCacheConnectionString
                ConnectionString = $"{_config.RedisCacheConnectionString.Replace("unit-test", "unit-test-flush")},allowAdmin=true,defaultDatabase=3"
            };

            //config.Name = "unit-test-flush";
            //config.Database = 3;
            //config.AllowAdmin = true;

            var provider = TestHelper.BuildRedisProvider(config, TimeSpan.FromMinutes(2), "async-flush-tests");

            var cacheService = provider.GetRequiredService<IOltCacheService>();
            var _redisFactory = provider.GetRequiredService<IRedisClientFactory>();

            var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
            var test2 = _redisFactory.GetAllClients();
            var test3 = _redisFactory.GetRedisDatabase();
            var test = redisDatabase.Database;

            for (var idx = 0; idx < 10; idx++)
            {
                var model = TestHelper.CreateModel();
                var cacheKey = $"cache-person-{Guid.NewGuid()}";
                cacheKeys.Add(cacheKey, model);
                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model))).Should().BeEquivalentTo(model);
            }

            foreach (var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(expected);
            }

            await cacheService.FlushAsync();

            foreach (var item in cacheKeys)
            {
                var newModel = TestHelper.CreateModel();
                var cacheKey = item.Key;
                var expected = item.Value;
                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(newModel);
            }
        }
    }
}