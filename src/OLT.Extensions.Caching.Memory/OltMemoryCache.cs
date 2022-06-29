using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public class OltMemoryCache : OltCacheService
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

        
        public override TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null)
        {
            var cacheEntry = _memoryCache.GetOrCreate(ToCacheKey(key), entry =>
            {
                if (absoluteExpiration.HasValue)
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpiration.Value);
                }
                else if (_cacheOptions.DefaultAbsoluteExpiration.HasValue)
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheOptions.DefaultAbsoluteExpiration.Value);
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
                      else if (_cacheOptions.DefaultAbsoluteExpiration.HasValue)
                      {
                          entry.AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheOptions.DefaultAbsoluteExpiration.Value);
                      }

                      return await factory();
                  });
            return cacheEntry;
        }
    }
}
