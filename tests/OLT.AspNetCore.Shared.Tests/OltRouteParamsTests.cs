using OLT.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.AspNetCore.Shared.Tests
{



    public class OltRouteParamsTests
    {
    
        [Theory]
        [InlineData("1234", 1234, true)]
        [InlineData("abc", 0, false)]
        [InlineData("", 0, false)]
        [InlineData(" ", 0, false)]
        [InlineData(null, 0, false)]
        public void OltRouteParamsParserIntTests(string param, int expectedValue, bool expectedResult)
        {
            var parser = new OltRouteParamsParserInt();
            int value;
            var result = parser.TryParse(param, out value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("1234", "1234", true)]
        [InlineData("abc", "abc", true)]
        [InlineData("", null, false)]
        [InlineData(" ", null, false)]
        [InlineData(null, null, false)]
        public void OltRouteParamsParserStringTests(string param, string expectedValue, bool expectedResult)
        {
            var parser = new OltRouteParamsParserString();
            string value;
            var result = parser.TryParse(param, out value);

            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedValue, value);
        }

        [Theory, MemberData(nameof(Guids))]
        public void OltRouteParamsParserGuidTests(string param, Guid expectedValue, bool expectedResult, bool shouldEqual)
        {
            
            var parser = new OltRouteParamsParserGuid();
            Guid value;
            var result = parser.TryParse(param, out value);

            Assert.Equal(expectedResult, result);
            
            if (shouldEqual)
            {
                Assert.Equal(expectedValue, value);
            }            
            
        }


        public static IEnumerable<object[]> Guids
        {
            get
            {
                yield return new object[] { " ", Guid.NewGuid(), false, false };
                yield return new object[] { "", Guid.NewGuid(), false, false };
                yield return new object[] { null, Guid.NewGuid(), false, false };
                yield return new object[] { "ABC-61729207-B012-440A-92CF-4AEA38C16D5D", Guid.NewGuid(), false, false };
                yield return new object[] { "274177A8-75B5-46A3-9804-D775941A6647", Guid.Parse("274177A8-75B5-46A3-9804-D775941A6647"), true, true };
            }
        }
    }
}