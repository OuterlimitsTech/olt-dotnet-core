using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace OLT.Core
{
    [Obsolete("Removing in 10.x, Move to FusionCache")]
    public class OltMemoryCache : OltCacheService, IOltMemoryCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly OltCacheOptions _cacheOptions;

        public OltMemoryCache(
            IOptions<OltCacheOptions> options,
            IMemoryCache memoryCache)
        {
            _cacheOptions = options.Value;
            _memoryCache = memoryCache;            
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(ToCacheKey(key));
        }

        public override Task RemoveAsync(string key)
        {
            _memoryCache.Remove(ToCacheKey(key));
            return Task.CompletedTask;
        }

        public override TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null)
        {            
            var cacheEntry = _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
            {
                if (absoluteExpiration.HasValue)
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpiration.Value);
                }
                else
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheOptions.DefaultAbsoluteExpiration);
                }
                return factory();
            });

            return cacheEntry;
        }

        public override async Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null)
        {
            var cacheEntry = await
                  _memoryCache.GetOrCreateAsync(ToCacheKey(key), async entry =>
                  {
                      if (absoluteExpiration.HasValue)
                      {
                          entry.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpiration.Value);
                      }
                      else
                      {
                          entry.AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheOptions.DefaultAbsoluteExpiration);
                      }

                      return await factory();
                  });
            return cacheEntry;
        }

        public override bool Exists(string key)
        {
            if (_memoryCache.TryGetValue(ToCacheKey(key), out object value))
            {
                return true;
            }
            return false;
        }

        public override Task<bool> ExistsAsync(string key)
        {
            if (_memoryCache.TryGetValue(ToCacheKey(key), out object value))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public override void Flush()
        {
            if (_memoryCache is MemoryCache memoryCache)
            {
                var percentage = 1.0; //100%
                memoryCache.Compact(percentage);
            }
        }

        public override Task FlushAsync()
        {
            Flush();
            return Task.CompletedTask;
        }
    }
}
