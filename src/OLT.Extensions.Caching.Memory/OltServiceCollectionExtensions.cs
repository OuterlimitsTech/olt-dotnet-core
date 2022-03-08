﻿using Microsoft.Extensions.Caching.Memory;
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
        /// Registers <see cref="IOltMemoryCache"/> as a singleton
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="defaultSlidingExpiration">Default expire cache after sliding Expiration. (uses default if not supplied)</param>
        /// <param name="defaultAbsoluteExpiration">Default expire cache at. (uses default if not supplied)</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddOltAddMemoryCache(this IServiceCollection services, TimeSpan defaultSlidingExpiration, TimeSpan defaultAbsoluteExpiration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services
                .AddOltAddMemoryCache(o =>
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(defaultSlidingExpiration)
                        .SetAbsoluteExpiration(defaultAbsoluteExpiration));
        }

        /// <summary>
        /// Adds Memory Cache
        /// </summary>
        /// <remarks>
        /// Registers <see cref="IOltMemoryCache"/> as a singleton
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="setupAction"></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddOltAddMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> setupAction) 
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return services
                .AddSingleton<IOltMemoryCache, OltMemoryCache>()
                .AddMemoryCache(setupAction);
        }
    }
}
