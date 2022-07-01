using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{

    public interface IOltCacheService : IOltInjectableSingleton
    {
        /// <summary>
        /// A generic method for getting and setting objects to the cache
        /// </summary>
        /// <typeparam name="TEntry">The type of the object to be returned.</typeparam>
        /// <param name="key">The name to be used when storing this object in the cache.</param>
        /// <param name="factory">A parameterless function to call if the object isn't in the cache and you need to set it.</param>
        /// <param name="absoluteExpiration">Expire cache at. (uses default if not supplied)</param>
        /// <returns>An object of the type you asked for</returns>
        TEntry Get<TEntry>(string key, Func<TEntry> factory, TimeSpan? absoluteExpiration = null);


        /// <summary>
        /// A generic method for getting and setting objects to the cache.
        /// </summary>
        /// <typeparam name="TEntry">The type of the object to be returned.</typeparam>
        /// <param name="key">The name to be used when storing this object in the cache.</param>
        /// <param name="factory">A parameterless function to call if the object isn't in the cache and you need to set it.</param>
        /// <param name="absoluteExpiration">Expire cache at. (uses default if not supplied)</param>
        /// <returns>An object of the type you asked for</returns>
        Task<TEntry> GetAsync<TEntry>(string key, Func<Task<TEntry>> factory, TimeSpan? absoluteExpiration = null);


        /// <summary>
        /// A generic method for getting and setting objects to the cache.
        /// </summary>
        /// <param name="key">The name to be used for this object in the cache.</param>
        void Remove(string key);

        /// <summary>
        /// A generic method for getting and setting objects to the cache.
        /// </summary>
        /// <param name="key">The name to be used for this object in the cache.</param>
        Task RemoveAsync(string key);
    }

}