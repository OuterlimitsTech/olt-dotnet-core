using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace OLT.Core
{
    public static class OltRedisCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Redis Cache Service
        /// </summary>
        /// <remarks>
        /// Registers <see cref="IOltCacheService"/> as a singleton to <see cref="OltRedisCache"/> and <see cref="IConnectionMultiplexer"/>
        /// </remarks>
        /// <typeparam name="T">Cache object serializer</typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="defaultAbsoluteExpiration">Default expire cache.</param>
        /// <param name="configurationConnectionString"> The string configuration to use for this multiplexer. ("localhost:6379,serviceName=application2,name=my-test-app2,defaultDatabase=3")</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddOltCacheRedis<T>(this IServiceCollection services, TimeSpan defaultAbsoluteExpiration, string configurationConnectionString)
            where T : class, IOltCacheSerializer, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(configurationConnectionString))
            {
                throw new ArgumentNullException(nameof(configurationConnectionString));
            }

            return services
                .AddSingleton<IOltCacheService, OltRedisCache>()
                .AddSingleton<IOltCacheSerializer>(opt => new T())
                .AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(configurationConnectionString))
                .Configure<OltCacheOptions>(opt =>
                {
                    opt.DefaultAbsoluteExpiration = defaultAbsoluteExpiration;
                });                
        }

        /// <summary>
        /// Adds Redis Cache Service
        /// </summary>
        /// <remarks>
        /// Registers <see cref="IOltCacheService"/> as a singleton to <see cref="OltRedisCache"/> and <see cref="IConnectionMultiplexer"/>
        /// </remarks>
        /// <typeparam name="T">Cache object serializer</typeparam>
        /// <param name="services"></param>
        /// <param name="defaultAbsoluteExpiration"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddOltCacheRedis<T>(this IServiceCollection services, TimeSpan defaultAbsoluteExpiration, ConfigurationOptions options)
            where T : class, IOltCacheSerializer, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return services
                .AddSingleton<IOltCacheService, OltRedisCache>()
                .AddSingleton<IOltCacheSerializer>(opt => new T())
                .AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(options))
                .Configure<OltCacheOptions>(opt =>
                {
                    opt.DefaultAbsoluteExpiration = defaultAbsoluteExpiration;
                });
        }

    }
}
