using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OLT.Core
{

    public static class OltMemoryCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Memory Cache
        /// </summary>
        /// <remarks>
        /// Registers <see cref="IOltCacheService"/> as a singleton to <see cref="OltMemoryCache"/>
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="defaultAbsoluteExpiration">Default expire cache at. (uses default if not supplied)</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        [Obsolete("Removing in 10.x, Move to FusionCache")]
        public static IServiceCollection AddOltCacheMemory(this IServiceCollection services, TimeSpan defaultAbsoluteExpiration)
        {
            ArgumentNullException.ThrowIfNull(services);

            return services
                .AddSingleton<IOltCacheService, OltMemoryCache>()
                .Configure<OltCacheOptions>(opt =>
                {
                    opt.DefaultAbsoluteExpiration = defaultAbsoluteExpiration;
                })
                .AddMemoryCache(opt => new MemoryCacheEntryOptions().SetAbsoluteExpiration(defaultAbsoluteExpiration));
        }
    }
}
