//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;
//using OLT.Core;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Testcontainers.Redis;
//using Xunit;

//namespace OLT.Extensions.Caching.Tests
//{
//    public class RedisCacheFlushTests : IAsyncLifetime
//    {
//        private readonly RedisContainer _redis = new RedisBuilder().Build();

//        public async Task InitializeAsync()
//        {
//            await _redis.StartAsync();
//        }

//        public async Task DisposeAsync()
//        {
//            await _redis.StopAsync();
//        }


//        [Fact]
//        public void FlushTests()
//        {

//            var cacheKeys = new Dictionary<string, OltPersonName>();
//            var services = new ServiceCollection();
//            //var config = new StackExchange.Redis.Extensions.Core.Configuration.RedisConfiguration
//            //{
//            //    ConnectionString = $"{_config.RedisCacheConnectionString.Replace("unit-test", "unit-test-flush")},allowAdmin=true,defaultDatabase=4"
//            //};

//            var provider = TestHelper.BuildRedisProvider(_redis, TimeSpan.FromMinutes(2), "flush-tests");

//            var cacheService = provider.GetRequiredService<IOltCacheService>();

//            for (var idx = 0; idx < 10; idx++)
//            {
//                var model = TestHelper.CreateModel();
//                var cacheKey = $"cache-person-{Guid.NewGuid()}";
//                cacheKeys.Add(cacheKey, model);
//                cacheService.Get(cacheKey, () => TestHelper.CloneModel(model)).Should().BeEquivalentTo(model);
//            }

//            foreach (var item in cacheKeys)
//            {
//                var newModel = TestHelper.CreateModel();
//                var cacheKey = item.Key;
//                var expected = item.Value;
//                cacheService.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(expected);
//            }

//            cacheService.Flush();

//            foreach (var item in cacheKeys)
//            {
//                var newModel = TestHelper.CreateModel();
//                var cacheKey = item.Key;
//                var expected = item.Value;
//                cacheService.Get(cacheKey, () => TestHelper.CloneModel(newModel)).Should().BeEquivalentTo(newModel);
//            }
//        }

//        [Fact]
//        public async Task FlushAsyncTests()
//        {

//            var cacheKeys = new Dictionary<string, OltPersonName>();
//            var services = new ServiceCollection();

//            //var config = new StackExchange.Redis.Extensions.Core.Configuration.RedisConfiguration
//            //{
//            //    ConnectionString = $"{_config.RedisCacheConnectionString.Replace("unit-test", "unit-test-flush-async")},allowAdmin=true,defaultDatabase=3"
//            //};

//            var provider = TestHelper.BuildRedisProvider(_redis, TimeSpan.FromMinutes(2), "async-flush-tests");

//            var cacheService = provider.GetRequiredService<IOltCacheService>();

//            for (var idx = 0; idx < 10; idx++)
//            {
//                var model = TestHelper.CreateModel();
//                var cacheKey = $"cache-person-{Guid.NewGuid()}";
//                cacheKeys.Add(cacheKey, model);
//                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(model))).Should().BeEquivalentTo(model);
//            }

//            foreach (var item in cacheKeys)
//            {
//                var newModel = TestHelper.CreateModel();
//                var cacheKey = item.Key;
//                var expected = item.Value;
//                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(expected);
//            }

//            await cacheService.FlushAsync();

//            foreach (var item in cacheKeys)
//            {
//                var newModel = TestHelper.CreateModel();
//                var cacheKey = item.Key;
//                var expected = item.Value;
//                (await cacheService.GetAsync(cacheKey, async () => await TestHelper.FakeAsync(newModel))).Should().BeEquivalentTo(newModel);
//            }
//        }

//    }
//}