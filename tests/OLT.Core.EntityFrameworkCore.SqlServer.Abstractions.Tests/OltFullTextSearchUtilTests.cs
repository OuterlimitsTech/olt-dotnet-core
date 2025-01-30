using System.ComponentModel;

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests;

public class OltFullTextSearchUtilTests
{
    [Theory]
    [InlineData("test", OltFtsWildCardType.None, false, "test")]
    [InlineData("test", OltFtsWildCardType.BeginsWith, false, "\"test*\"")]
    [InlineData("test", OltFtsWildCardType.EndsWith, false, "\"*test\"")]
    [InlineData("test", OltFtsWildCardType.Contains, false, "\"*test*\"")]
    [InlineData("test search", OltFtsWildCardType.None, true, "test and search")]
    [InlineData("test search", OltFtsWildCardType.None, false, "test or search")]
    public void Contains_ShouldReturnExpectedResult(string search, OltFtsWildCardType wildCardType, bool matchAllWords, string expected)
    {
        var result = OltFullTextSearchUtil.Contains(search, wildCardType, matchAllWords);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("test", false, "test")]
    [InlineData("test search", true, "test and search")]
    [InlineData("test search", false, "test or search")]
    public void FreeText_ShouldReturnExpectedResult(string search, bool matchAllWords, string expected)
    {
        var result = OltFullTextSearchUtil.FreeText(search, matchAllWords);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("test", OltFtsWildCardType.None, "test")]
    [InlineData("test", OltFtsWildCardType.BeginsWith, "\"test*\"")]
    [InlineData("test", OltFtsWildCardType.EndsWith, "\"*test\"")]
    [InlineData("test", OltFtsWildCardType.Contains, "\"*test*\"")]
    public void FormatWordWildCard_ShouldReturnExpectedResult(string word, OltFtsWildCardType wildCardType, string expected)
    {
        var result = OltFullTextSearchUtil.FormatWordWildCard(word, wildCardType);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatWordWildCard_ShouldThrowInvalidEnumArgumentException()
    {
        var invalidWildCardType = (OltFtsWildCardType)999;

        Assert.Throws<InvalidEnumArgumentException>(() => OltFullTextSearchUtil.FormatWordWildCard("test", invalidWildCardType));
    }
}
