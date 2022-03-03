using System.ComponentModel;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests.CustomAttributeTests
{
    public class CodeAttributeTests
    {
        public enum CodeAttributeTest
        {
            [Code("value-1", 10)]
            Value1,

            Value2,

            [Description("Value 3")]
            Value3,

            [Description("Value 4")]
            [Code("value-4", 5000)]
            Value4,
        }

        private const short DefaultSort = 9999;

        [Fact]
        public void RawAttributeTest()
        {
            var value = Faker.Lorem.Words(20).Last();

            var attr = new CodeAttribute(value);
            Assert.Equal(value, attr.Code);
            Assert.Equal(DefaultSort, attr.DefaultSort);

            value = Faker.Lorem.Words(10).Last();
            var sort = (short)Faker.RandomNumber.Next(100, 500);
            attr = new CodeAttribute(value, sort);
            Assert.Equal(value, attr.Code);
            Assert.Equal(sort, attr.DefaultSort);
        }

        [Theory]
        [InlineData("value-1", (short)10, CodeAttributeTest.Value1)]
        [InlineData(null, null, CodeAttributeTest.Value2)]
        [InlineData(null, null, CodeAttributeTest.Value3)]
        [InlineData("value-4", (short)5000, CodeAttributeTest.Value4)]
        [InlineData(null, null, null)]
        public void GetCodeEnumExtenstions(string expected, short? expectedSort, CodeAttributeTest? value)
        {
            Assert.Equal(expected, OltCodeAttributeExtensions.GetCodeEnum(value));
            Assert.Equal(expectedSort, OltCodeAttributeExtensions.GetCodeEnumSort(value));
        }


        [Theory]
        [InlineData(CodeAttributeTest.Value1, "value-1")]
        [InlineData(CodeAttributeTest.Value4, "value-4")]
        [InlineData(null, null)]
        public void FromCodeEnumExtenstions(CodeAttributeTest? expected, string value)
        {
            if (value == null)
            {
                Assert.Throws<System.ArgumentNullException>(() => OltCodeAttributeExtensions.FromCodeEnum<CodeAttributeTest>(value));
            }
            else
            {
                Assert.Equal(expected, OltCodeAttributeExtensions.FromCodeEnum<CodeAttributeTest>(value));
            }            
        }
    }
}
