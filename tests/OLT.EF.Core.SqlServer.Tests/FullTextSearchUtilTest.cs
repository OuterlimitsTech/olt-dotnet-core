using OLT.Core;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace OLT.EF.Core.SqlServer.Tests
{
    public class FullTextSearchUtilTest
    {
        public static class PersonNames
        {
            public static class Name1
            {
                public const string First = "Ava";
                public const string Last = "Larson";
                public const string FullName = "Ava Larson";
            }

            public static class Name2
            {
                public const string First = "Alban";
                public const string Last = "Trivett";
                public const string FullName = "Alban Trivett";
            }
        }

        private static string FormatResult(string search, OltFtsWildCardType wildCardType, bool matchAllWords)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return search;
            }

            search = search.CleanForSearch();
            var words = search.Split(' ', '　', StringSplitOptions.None).ToList();

            return matchAllWords
                ? string.Join(" and ", words.Where(c => c != "and").Select(word => OltFullTextSearchUtil.FormatWordWildCard(word, wildCardType)))
                : string.Join(" or ", words.Where(c => c != "or").Select(word => OltFullTextSearchUtil.FormatWordWildCard(word, wildCardType)));
        }

        [Theory]
        [InlineData(PersonNames.Name1.First, OltFtsWildCardType.BeginsWith, false, true)]
        [InlineData(PersonNames.Name2.First, OltFtsWildCardType.BeginsWith, true, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.BeginsWith, false, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.BeginsWith, true, true)]
        [InlineData(null, OltFtsWildCardType.BeginsWith, false, true)]
        [InlineData(null, OltFtsWildCardType.BeginsWith, true, true)]
        [InlineData("", OltFtsWildCardType.BeginsWith, false, true)]
        [InlineData("", OltFtsWildCardType.BeginsWith, true, true)]
        [InlineData(" ", OltFtsWildCardType.BeginsWith, false, true)]
        [InlineData(" ", OltFtsWildCardType.BeginsWith, true, true)]

        [InlineData(PersonNames.Name1.First, OltFtsWildCardType.EndsWith, false, true)]
        [InlineData(PersonNames.Name2.First, OltFtsWildCardType.EndsWith, true, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.EndsWith, false, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.EndsWith, true, true)]
        [InlineData(null, OltFtsWildCardType.EndsWith, false, true)]
        [InlineData(null, OltFtsWildCardType.EndsWith, true, true)]
        [InlineData("", OltFtsWildCardType.EndsWith, false, true)]
        [InlineData("", OltFtsWildCardType.EndsWith, true, true)]
        [InlineData(" ", OltFtsWildCardType.EndsWith, false, true)]
        [InlineData(" ", OltFtsWildCardType.EndsWith, true, true)]

        [InlineData(PersonNames.Name1.First, OltFtsWildCardType.Contains, false, true)]
        [InlineData(PersonNames.Name2.First, OltFtsWildCardType.Contains, true, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.Contains, false, true)]
        [InlineData(PersonNames.Name2.FullName, OltFtsWildCardType.Contains, true, true)]
        [InlineData(null, OltFtsWildCardType.Contains, false, true)]
        [InlineData(null, OltFtsWildCardType.Contains, true, true)]
        [InlineData("", OltFtsWildCardType.Contains, false, true)]
        [InlineData("", OltFtsWildCardType.Contains, true, true)]
        [InlineData(" ", OltFtsWildCardType.Contains, false, true)]
        [InlineData(" ", OltFtsWildCardType.Contains, true, true)]
        public void WildCardType(string value, OltFtsWildCardType widCardType, bool matchAllWords, bool expectedResult)
        {
            var result = OltFullTextSearchUtil.Contains(value, widCardType, matchAllWords);
            var compare = FormatResult(value, widCardType, matchAllWords);

            if (expectedResult)
            {
                Assert.Equal(result, compare);
            }
            else
            {
                Assert.NotEqual(result, compare);
            }
        }

        [Fact]
        public void Invalid()
        {
            var word = Faker.Name.First();
            var invalidValue = -1000;
            Assert.Throws<InvalidEnumArgumentException>(() => OltFullTextSearchUtil.Contains(word, (OltFtsWildCardType)invalidValue));
        }

        [Fact]
        public void FreeText()
        {
            var word = Faker.Name.First();
            var firstName = OltFullTextSearchUtil.FreeText(word);
            Assert.True(firstName.Equals(word.CleanForSearch()));
        }

    }
}