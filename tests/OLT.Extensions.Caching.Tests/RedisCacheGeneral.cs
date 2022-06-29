using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using StackExchange.Redis;
using System;
using Xunit;

namespace OLT.Extensions.Caching.Tests
{
    public class RedisCacheGeneral
    {

        [Fact]
        public void NewtonsoftCacheSerializerTests()
        {
            var model = TestHelper.CreateModel();
            var serializer = new OltNewtonsoftCacheSerializer();
            var result = serializer.Serialize(model);
            Assert.NotNull(result);
            var clone = serializer.Deserialize<OltPersonName>(result);
            clone.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void ExtensionExceptionTests()
        {
            var services = new ServiceCollection();
            string connectionString = null;

            Assert.Throws<ArgumentNullException>("services", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(null, TimeSpan.FromSeconds(15), ""));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), connectionString));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), ""));
            Assert.Throws<ArgumentNullException>("configurationConnectionString", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), " "));

            ConfigurationOptions options = null;
            Assert.Throws<ArgumentNullException>("options", () => OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), options));

            try
            {
                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), "localhost:6379");

                options = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { "localhost", 6379 },
                    },
                    ClientName = "test-app-2",
                };

                OltRedisCacheServiceCollectionExtensions.AddOltCacheRedis<OltNewtonsoftCacheSerializer>(services, TimeSpan.FromSeconds(15), options);
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }
        }
    }
}