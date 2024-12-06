using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.Extensions.Caching.Tests;

public class OltMemoryCacheTests
{

    [Fact]
    public void Get_ReturnsValueFromMemoryCache()
    {


        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        var result = cache.Get(key, () => value);
        Assert.Equal(value, result);
    }

    [Fact]
    public async Task GetAsync_ReturnsValueFromMemoryCache()
    {

        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        var result = await cache.GetAsync(key, () => Task.FromResult(value));
        Assert.Equal(value, result);
    }

    [Fact]
    public void Remove_RemovesValueFromMemoryCache()
    {
        
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        var result = cache.Get(key, () => value);
        Assert.True(cache.Exists(key));
        cache.Remove(key);
        Assert.False(cache.Exists(key));
    }

    [Fact]
    public async Task RemoveAsync_RemovesValueFromMemoryCache()
    {
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        var result = await cache.GetAsync(key, () => Task.FromResult(value));
        Assert.True(cache.Exists(key));
        await cache.RemoveAsync(key);
        Assert.False(cache.Exists(key));
    }

    [Fact]
    public void Exists_ReturnsTrueWhenValueExistsInMemoryCache()
    {
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        _ = cache.Get(key, () => value);
        var result = cache.Exists(key);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrueWhenValueExistsInMemoryCache()
    {
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        _ = await cache.GetAsync(key, () => Task.FromResult("defaultValue"));
        var result = await cache.ExistsAsync(key);
        Assert.True(result);
    }

    [Fact]
    public void Flush_CallsCompactOnMemoryCache()
    {
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        _ = cache.Get(key, () => value);
        Assert.True(cache.Exists(key));
        cache.Flush();
        Assert.False(cache.Exists(key));
    }

    [Fact]
    public async Task FlushAsync_CallsCompactOnMemoryCache()
    {
        var provider = TestHelper.BuildMemoryProvider(TimeSpan.FromSeconds(2));
        var cache = provider.GetRequiredService<IOltCacheService>();

        var key = "testKey";
        var value = "testValue";
        var result = await cache.GetAsync(key, () => Task.FromResult(value));
        Assert.True(cache.Exists(key));
        await cache.FlushAsync();
        Assert.False(cache.Exists(key));
    }


    [Fact]
    public void AddOltCacheMemory_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>("services", () => OltMemoryCacheServiceCollectionExtensions.AddOltCacheMemory(null, TimeSpan.FromSeconds(15)));
    }

}
