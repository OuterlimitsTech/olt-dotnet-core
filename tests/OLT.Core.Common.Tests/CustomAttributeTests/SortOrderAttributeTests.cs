using OLT.Constants;
using System.ComponentModel;
using Xunit;

namespace OLT.Core.Common.Tests.CustomAttributeTests
{
    public class SortOrderAttributeTests
    {
        public enum SortAttributeTest
        {
            [Code("value-1", 10)]
            [SortOrder(15)]
            Value1,

            Value2,

            [Description("Value 3")]
            [SortOrder(987)]
            Value3,

            [Description("Value 4")]
            [Code("value-4", 5000)]
            Value4,
        }

        private const short DefaultSort = OltCommonDefaults.SortOrder;

        [Fact]
        public void RawAttributeTest()
        {
            var value = (short)Faker.RandomNumber.Next(short.MaxValue);
            var attr = new SortOrderAttribute(value);
            Assert.Equal(value, attr.SortOrder);
            Assert.Equal(OltCommonDefaults.SortOrder, new SortOrderAttribute().SortOrder);
        }

        [Theory]
        [InlineData((short)15, SortAttributeTest.Value1)]
        [InlineData(OltCommonDefaults.SortOrder, SortAttributeTest.Value2)]
        [InlineData((short)987, SortAttributeTest.Value3)]
        [InlineData(OltCommonDefaults.SortOrder, SortAttributeTest.Value4)]
        [InlineData(OltCommonDefaults.SortOrder, null)]
        public void GetSortOrderEnumExtenstions(short? expectedSort, SortAttributeTest? value)
        {
            Assert.Equal(expectedSort, OltSortOrderAttributeExtensions.GetSortOrderEnum(value));            
        }


        [Theory]
        [InlineData((short)15, SortAttributeTest.Value1, (short)1987)]
        [InlineData((short)1765, SortAttributeTest.Value2, (short)1765)]
        [InlineData((short)987, SortAttributeTest.Value3, (short)1567)]
        [InlineData((short)1456, SortAttributeTest.Value4, (short)1456)]
        [InlineData((short)8766, null, (short)8766)]
        public void GetSortOrderEnumExtenstionsWithDefault(short? expectedSort, SortAttributeTest? value, short defaultValue)
        {
            Assert.Equal(expectedSort, OltSortOrderAttributeExtensions.GetSortOrderEnum(value, defaultValue));
        }
    }
}
