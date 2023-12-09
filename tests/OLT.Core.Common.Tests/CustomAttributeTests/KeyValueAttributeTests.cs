using System;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests.CustomAttributeTests
{
    public class KeyValueAttributeTests
    {
        public enum TestValue
        {
            [KeyValue("key1", "value1")]
            Single,
            
            NoAttribute,

            [KeyValue("key1", "value1")]
            [KeyValue("key2", "value2")]
            [KeyValue("key3", "value3")]
            Muliple,

        }

        [Fact]
        public void RawAttributeTest()
        {
            var key = Faker.Lorem.Words(9).Last();
            var value = Faker.Lorem.Words(15).Last();
            var attr = new KeyValueAttribute(key, value);
            Assert.Equal(key, attr.Key);
            Assert.Equal(value, attr.Value);
            Assert.Throws<ArgumentNullException>(() => new KeyValueAttribute(null, null));
            Assert.Throws<ArgumentNullException>(() => new KeyValueAttribute(null, value));
            Assert.Null(new KeyValueAttribute(key, null).Value);
        }

        [Theory]
        [InlineData(1, TestValue.Single)]
        [InlineData(0, TestValue.NoAttribute)]
        [InlineData(3, TestValue.Muliple)]
        public void Count(int expected, TestValue @enum)
        {
            Assert.Equal(expected, OltKeyValueAttributeExtensions.GetKeyValueAttributes(@enum)?.Count);
        }

        [Fact]
        public void Null()
        {
            Assert.NotNull(OltKeyValueAttributeExtensions.GetKeyValueAttributes(null));
            Assert.Empty(OltKeyValueAttributeExtensions.GetKeyValueAttributes(null));
        }

    }
}
