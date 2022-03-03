using System;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests.CustomAttributeTests
{
    public class UniqueIdAttributeTests
    {
        public enum TestValue
        {
            [UniqueId("1393fff9-3850-4bb2-848b-18973a9f88d0")]
            Valid,

            NoAttrib,
        }
        
        [Fact]
        public void RawAttributeTest()
        {
            var value = Guid.NewGuid();
            var attr = new UniqueIdAttribute(value.ToString());
            Assert.Equal(value, attr.UniqueId);
            Assert.Throws<FormatException>(() => new UniqueIdAttribute(Faker.Lorem.Words(10).Last()));
        }

        [Theory]
        [InlineData("1393fff9-3850-4bb2-848b-18973a9f88d0", TestValue.Valid)]
        [InlineData(null, TestValue.NoAttrib)]
        public void GetCodeEnumExtenstions(string expected, TestValue value)
        {
            Guid? guid = null;
            try
            {
                guid = new Guid(expected);
            }
            catch 
            {
                // ignore
            }

            var uid = OltAttributeExtensions.GetAttributeInstance<UniqueIdAttribute, TestValue>(value)?.UniqueId;
            Assert.Equal(guid, uid);
        
        }
    }
}
