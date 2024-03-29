﻿using OLT.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using OLT.Extensions.Caching.Tests.Assets;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using Microsoft.Extensions.Logging;

namespace OLT.Extensions.Caching.Tests
{
    public static class TestHelper
    {
        public static Task<OltPersonName> FakeAsync(OltPersonName model)
        {
            return Task.FromResult(model);
        }

        public static OltPersonName CreateModel()
        {
            return new OltPersonName
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
        }

        public static OltPersonName CreateModel(string key)
        {
            return new OltPersonName
            {
                First = Faker.Name.First(),
                Middle = key,
                Last = Faker.Name.Last(),
            };
        }


        public static OltPersonName CloneModel(OltPersonName from)
        {
            return new OltPersonName
            {
                First = from.First,
                Middle = from.Middle,
                Last = from.Last,
                Suffix = from.Suffix,
            };
        }

        public static ServiceProvider BuildMemoryProvider(TimeSpan defaultAbsoluteExpiration)
        {
            var services = new ServiceCollection();
            services.AddOltCacheMemory(defaultAbsoluteExpiration);
            return services.BuildServiceProvider();
        }

        public static ServiceProvider BuildRedisProvider(CacheConfiguration configuration, TimeSpan defaultAbsoluteExpiration, string cacheKeyPrefix)
        {
            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
            services.AddOltCacheRedis(defaultAbsoluteExpiration, cacheKeyPrefix, configuration.RedisCacheConnectionString);
            return services.BuildServiceProvider();
        }

        public static ServiceProvider BuildRedisProvider(RedisConfiguration configuration, TimeSpan defaultAbsoluteExpiration, string cacheKeyPrefix)
        {
            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
            services.AddOltCacheRedis(defaultAbsoluteExpiration, cacheKeyPrefix, configuration);
            return services.BuildServiceProvider();
        }

    }
}