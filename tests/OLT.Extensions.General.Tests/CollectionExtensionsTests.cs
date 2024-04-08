using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Extensions.General.Tests
{

    public class CollectionExtensionsTests
    {

        public static IEnumerable<object[]> JoinMemberData =>
            new List<object[]>
            {
                new object[] { new List<string> { "Item1", "Item2" }, ",", "Item1,Item2" },
                new object[] { new List<string> { null, "Item2" }, ",", ",Item2" },
                new object[] { new List<string> { "Item1", "Item2" }, null, "Item1Item2" },
                new object[] { new List<string> { "Item1", "Item2" }, "", "Item1Item2" },
            };

        [Theory]
        [MemberData(nameof(JoinMemberData))]
        public void Join(List<string> values, string separator, string expectedResult)
        {
            Assert.Equal(expectedResult, values.Join(separator));
        }


        public static IEnumerable<object[]> DelimitedStringData =>
            new List<object[]>
            {
                new object[] { new HelperToDelimitedString(",", false, "", "Item1", "Item2") },
                new object[] { new HelperToDelimitedString(",", true, "", "Item1", "Item2") },
                new object[] { new HelperToDelimitedString(",", true, "\"", "Item1", "Item2") },
                new object[] { new HelperToDelimitedString(",", false, "\"", "Item1", "Item2") },
                new object[] { new HelperToDelimitedString(":", true, "", "Item1", "Item2", "Item3", "Item4") },
                new object[] { new HelperToDelimitedString(":", false, "'", "Item1", "Item2", "Item6", "Item7", "Item9") },
                new object[] { new HelperToDelimitedString(null, true, "", "Item1", "Item2", "Item3", "Item4") },
                new object[] { new HelperToDelimitedString("-", true, null, "Item1", "Item2", "Item6", "Item7", "Item9") },
                new object[] { new HelperToDelimitedString(",", false, "") },
            };


        [Theory]
        [MemberData(nameof(DelimitedStringData))]
        [Obsolete]
        public void DelimitedString(HelperToDelimitedString request)
        {
            Assert.Equal(request.Expected, request.Values.ToDelimitedString(request.Delimiter, request.InsertSpaces, request.Qualifier));
        }

    }
}