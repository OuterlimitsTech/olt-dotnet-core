using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{
    public abstract class OltCacheService : OltDisposable, IOltCacheService
    {
        protected virtual string ToCacheKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.IsEmpty())
            {
                throw new ArgumentException(nameof(key));
            }

            return key.ToLower();
        }

        public abstract TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null);
        public abstract Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null);
        public abstract void Remove(string key);
        public abstract Task RemoveAsync(string key);
        public abstract bool Exists(string key);
        public abstract Task<bool> ExistsAsync(string key);
    }
}