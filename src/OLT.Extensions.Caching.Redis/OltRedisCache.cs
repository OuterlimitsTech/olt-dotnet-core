using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public class OltRedisCache : OltCacheService
    {
        private readonly OltRedisCacheOptions _cacheOptions;
        private readonly IRedisClientFactory _redisFactory;

        public OltRedisCache(
            IRedisClientFactory redisFactory,
            IOptions<OltRedisCacheOptions> options)
        {
            _cacheOptions = options.Value;
            _redisFactory = redisFactory;
        }

        protected override string ToCacheKey(string key)
        {
            return $"{_cacheOptions.CacheKeyPrefix}:{base.ToCacheKey(key)}".ToLower();
        }

        private string BuildKey<TEntry>(string key, Func<TEntry> factory)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return ToCacheKey(key);
        }

        public override TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null)
        {
            var cacheKey = BuildKey(key, factory);

            var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
            if (redisDatabase.ExistsAsync(cacheKey).GetAwaiter().GetResult())
            {
                return redisDatabase.GetAsync<TEntry>(cacheKey).GetAwaiter().GetResult();
            }

            TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

            var value = factory();
            if (expireAt.HasValue)
            {
                redisDatabase.AddAsync(cacheKey, value, expireAt.Value).GetAwaiter().GetResult();
            }
            else
            {
                redisDatabase.AddAsync(cacheKey, value).GetAwaiter().GetResult();
            }

            return redisDatabase.GetAsync<TEntry>(cacheKey).GetAwaiter().GetResult();
        }

        public override async Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null)
        {
            var cacheKey = BuildKey(key, factory);
            var redisDatabase = _redisFactory.GetDefaultRedisDatabase();

            if (await redisDatabase.ExistsAsync(cacheKey))
            {
                return await redisDatabase.GetAsync<TEntry>(cacheKey);
            }

            TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

            var value = await factory();
            if (expireAt.HasValue)
            {
                await redisDatabase.AddAsync(cacheKey, value, expireAt.Value);
            }
            else
            {
                await redisDatabase.AddAsync(cacheKey, value);
            }

            return await redisDatabase.GetAsync<TEntry>(cacheKey);
        }

        public override void Remove(string key)
        {
            var cacheKey = ToCacheKey(key);
            var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
            redisDatabase.RemoveAsync(cacheKey).GetAwaiter().GetResult();
        }

        public override async Task RemoveAsync(string key)
        {
            var cacheKey = ToCacheKey(key);
            var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
            await redisDatabase.RemoveAsync(cacheKey);
        }
    }
}
