using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OLT.Core;
using Testcontainers.Redis;

namespace OLT.Extensions.Caching.Tests;

public static class TestHelper
{      
    public static ServiceProvider BuildMemoryProvider(TimeSpan defaultAbsoluteExpiration)
    {
        var services = new ServiceCollection();
        services.AddOltCacheMemory(defaultAbsoluteExpiration);
        return services.BuildServiceProvider();
    }

    public static ServiceProvider BuildRedisProvider(RedisContainer container, TimeSpan defaultAbsoluteExpiration, string cacheKeyPrefix, string database = "3")
    {
        //var services = new ServiceCollection();
        //services.AddLogging(config => config.AddConsole());
        //var connStr = $"{container.GetConnectionString()},allowAdmin=true,defaultDatabase=3";
        //services.AddOltCacheRedis(defaultAbsoluteExpiration, cacheKeyPrefix, connStr);
        //return services.BuildServiceProvider();
        var connStr = $"{container.GetConnectionString()},allowAdmin=true,defaultDatabase={database}";
        return BuildRedisProvider(connStr, defaultAbsoluteExpiration, cacheKeyPrefix);
    }

    public static ServiceProvider BuildRedisProvider(string connectionString, TimeSpan defaultAbsoluteExpiration, string cacheKeyPrefix)
    {
        var services = new ServiceCollection();
        services.AddLogging(config => config.AddConsole());
        services.AddOltCacheRedis(defaultAbsoluteExpiration, cacheKeyPrefix, connectionString);
        return services.BuildServiceProvider();
    }

}