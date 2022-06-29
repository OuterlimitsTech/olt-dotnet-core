using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public class OltRedisCache : OltCacheService
    {
        private readonly IOltCacheSerializer _cacheSerializer;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly OltCacheOptions _cacheOptions;

        public OltRedisCache(
            IOptions<OltCacheOptions> options,
            IConnectionMultiplexer connectionMultiplexer,
            IOltCacheSerializer cacheSerializer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cacheSerializer = cacheSerializer;
            _cacheOptions = options.Value;
        }

        protected override string ToCacheKey(string key)
        {
            return $"{_connectionMultiplexer.ClientName}:{base.ToCacheKey(key)}";
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

            var db = _connectionMultiplexer.GetDatabase();
            var redisValue = db.StringGet(cacheKey);
            if (redisValue.HasValue)
            {
                return _cacheSerializer.Deserialize<TEntry>(redisValue.ToString());
            }

            TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

            var value = factory();
            var stringValue = _cacheSerializer.Serialize(value);
            db.StringSet(cacheKey, stringValue, expireAt);
            return value;
        }

        public override async Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null)
        {
            var cacheKey = BuildKey(key, factory);

            var db = _connectionMultiplexer.GetDatabase();
            var redisValue = await db.StringGetAsync(cacheKey);
            if (redisValue.HasValue)
            {
                return _cacheSerializer.Deserialize<TEntry>(redisValue.ToString());
            }
            
            TimeSpan? expireAt = absoluteExpiration ?? _cacheOptions.DefaultAbsoluteExpiration;

            var value = await factory();
            var stringValue = _cacheSerializer.Serialize(value);
            await db.StringSetAsync(cacheKey, stringValue, expireAt);


            return value;
        }

        public override void Remove(string key)
        {
            var cacheKey = ToCacheKey(key);
            var db = _connectionMultiplexer.GetDatabase();
            db.KeyDelete(cacheKey);
        }
    }
}
