using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using Testcontainers.Redis;

namespace OLT.Extensions.Caching.Tests;

/// <summary>
/// This test if we fail to connect to redis, it falls back to memory
/// </summary>
public class OltRedisCacheFallbackTests 
{      

    [Fact]
    public async Task Get_ReturnsValueFromRedis()
    {
        var key = "testKey";
        var value = "testValue";

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        var provider = TestHelper.BuildRedisProvider(redisContainer, TimeSpan.FromSeconds(20), "prefix:");
        var cacheService = provider.GetRequiredService<IOltCacheService>();
        await redisContainer.StopAsync();

        cacheService.Get(key, () => value);
        var result = cacheService.Get(key, () => "defaultValue");

        Assert.Equal(value, result);
    }

    [Fact]
    public async Task GetAsync_ReturnsValueFromRedis()
    {
        var key = "testKey";
        var value = "testValue";

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        var provider = TestHelper.BuildRedisProvider(redisContainer, TimeSpan.FromSeconds(20), "prefix:");
        var cacheService = provider.GetRequiredService<IOltCacheService>();
        await redisContainer.StopAsync();

        await cacheService.GetAsync(key, () => Task.FromResult(value));

        var result = await cacheService.GetAsync(key, () => Task.FromResult("defaultValue"));

        Assert.Equal(value, result);
    }

    [Fact]
    public async Task Remove_RemovesValueFromRedis()
    {
        var key = "testKey";
        var value = "testValue";

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        var provider = TestHelper.BuildRedisProvider(redisContainer, TimeSpan.FromSeconds(20), "prefix:");
        var cacheService = provider.GetRequiredService<IOltCacheService>();
        await redisContainer.StopAsync();

        cacheService.Get(key, () => value);
        cacheService.Remove(key);

        var result = cacheService.Exists(key);

        Assert.False(result);
    }

    [Fact]
    public async Task RemoveAsync_RemovesValueFromRedis()
    {
        var key = "testKey";
        var value = "testValue";

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        var provider = TestHelper.BuildRedisProvider(redisContainer, TimeSpan.FromSeconds(20), "prefix:");
        var cacheService = provider.GetRequiredService<IOltCacheService>();
        await redisContainer.StopAsync();

        await cacheService.GetAsync(key, () => Task.FromResult(value));
        await cacheService.RemoveAsync(key);

        var result = await cacheService.ExistsAsync(key);

        Assert.False(result);
    }

    [Fact]
    public async Task FlushAsync_CallsFlushAsync()
    {
        var key = "testKey";
        var value = "testValue";

        var redisContainer = new RedisBuilder().Build();
        await redisContainer.StartAsync();
        var provider = TestHelper.BuildRedisProvider(redisContainer, TimeSpan.FromSeconds(20), "prefix:");
        var cacheService = provider.GetRequiredService<IOltCacheService>();
        await redisContainer.StopAsync();


        await cacheService.GetAsync(key, () => Task.FromResult(value));
        await cacheService.FlushAsync();

        var result = await cacheService.ExistsAsync(key);

        Assert.False(result);
    }

}
