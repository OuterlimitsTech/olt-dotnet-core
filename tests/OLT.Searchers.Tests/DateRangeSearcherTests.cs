using FluentAssertions;
using FluentDateTimeOffset;
using OLT.Core.Searchers.Tests.Assets;
using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.Core.Searchers.Tests
{
    public class DateRangeSearcherTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void SearcherTests(FakeEntitySearcher searcher, OltDateRange expected, DateTimeOffset expectedQueryEnd)
        {
            searcher.Value.Should().BeEquivalentTo(expected);
            Assert.Equal(expectedQueryEnd, searcher.QueryEndValue);            
        }


        public static TheoryData<FakeEntitySearcher, OltDateRange, DateTimeOffset> Data
        {
            get
            {
                var now = DateTimeOffset.Now;

                var results = new TheoryData<FakeEntitySearcher, OltDateRange, DateTimeOffset>();
                results.Add(new FakeEntitySearcher(), new OltDateRange(), DateTimeOffset.MinValue.AddSeconds(1));
                results.Add(new FakeEntitySearcher(now.Midnight(), now.Midnight().AddDays(3)), new OltDateRange(now.Midnight(), now.Midnight().AddDays(3)), now.Midnight().AddDays(3).AddSeconds(1));
                results.Add(new FakeEntitySearcher(new OltDateRange(now, now.AddDays(4))), new OltDateRange(now, now.AddDays(4)), now.AddDays(4).AddSeconds(1));
                results.Add(new FakeEntitySearcher(OltDateRange.Today), OltDateRange.Today, OltDateRange.Today.End.AddSeconds(1));
                return results;
            }
        }
    }
}