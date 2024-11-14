﻿using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace OLT.Core
{
    public class OltRedisCache : OltCacheService
    {
        private readonly OltRedisCacheOptions _cacheOptions;
        private readonly IRedisClientFactory _redisFactory;
        private readonly IOltMemoryCache _memoryCache;

        public OltRedisCache(
            IRedisClientFactory redisFactory,
            IOptions<OltRedisCacheOptions> options,
            IOltMemoryCache memoryCache)
        {
            _cacheOptions = options.Value;
            _redisFactory = redisFactory;
            _memoryCache = memoryCache;
        }

        protected override string ToCacheKey(string key)
        {
            return $"{_cacheOptions.CacheKeyPrefix}:{base.ToCacheKey(key)}";
        }
             

        public override TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null)
        {
            ArgumentNullException.ThrowIfNull(factory);
            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                if (redisDatabase.ExistsAsync(cacheKey).GetAwaiter().GetResult())
                {
                    return redisDatabase.GetAsync<TEntry>(cacheKey).GetAwaiter().GetResult() ?? factory();
                }

                TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

                var value = factory();
                redisDatabase.AddAsync(cacheKey, value, expireAt.Value).GetAwaiter().GetResult();

                return redisDatabase.GetAsync<TEntry>(cacheKey).GetAwaiter().GetResult() ?? throw new ApplicationException("Invalid Cache Requests");
            }
            catch
            {
                return _memoryCache.Get(key, factory, absoluteExpiration);
            }
        }

        public override async Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null)
        {
            ArgumentNullException.ThrowIfNull(factory);

            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();

                if (await redisDatabase.ExistsAsync(cacheKey))
                {
                    return await redisDatabase.GetAsync<TEntry>(cacheKey) ?? await factory();
                }

                TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

                var value = await factory();
                await redisDatabase.AddAsync(cacheKey, value, expireAt.Value);

                return await redisDatabase.GetAsync<TEntry>(cacheKey) ?? throw new ApplicationException("Invalid Cache Requests");
            }
            catch
            {
                return await _memoryCache.GetAsync(key, factory, absoluteExpiration);
            }
        }

        public override void Remove(string key)
        {
            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                redisDatabase.RemoveAsync(cacheKey).GetAwaiter().GetResult();
            }
            catch
            {
                _memoryCache.Remove(key);
            }
        }

        public override async Task RemoveAsync(string key)
        {
            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                await redisDatabase.RemoveAsync(cacheKey);
            }
            catch
            {
                await _memoryCache.RemoveAsync(key);
            }
        }

        public override bool Exists(string key)
        {
            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                return redisDatabase.ExistsAsync(cacheKey).GetAwaiter().GetResult();
            }
            catch
            {
                return _memoryCache.Exists(key);
            }
        }

        public override async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var cacheKey = ToCacheKey(key);
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                return await redisDatabase.ExistsAsync(cacheKey);
            }
            catch
            {
                return await _memoryCache.ExistsAsync(key);
            }
        }

        public override void Flush()
        {
            FlushAsync().GetAwaiter().GetResult();            
        }

        public override async Task FlushAsync()
        {
            await _memoryCache.FlushAsync();

            try
            {
                var redisDatabase = _redisFactory.GetDefaultRedisDatabase();
                await redisDatabase.FlushDbAsync();
            }
            catch
            {
                //Do Nothing (Eat Error)
            }
            
        }
    }
}
