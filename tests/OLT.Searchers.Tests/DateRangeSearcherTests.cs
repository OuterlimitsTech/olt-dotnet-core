using AwesomeAssertions;
using FluentDateTimeOffset;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.Searchers.Tests
{
    public class DateRangeSearcherTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void SearcherTests(FakeEntityDateRangeSearcher searcher, OltDateRange expected, DateTimeOffset expectedQueryEnd)
        {
            searcher.Value.Should().BeEquivalentTo(expected, opt => opt.Excluding(o => o.Label));
            Assert.Equal(expectedQueryEnd, searcher.QueryEndValue);
        }


        public static TheoryData<FakeEntityDateRangeSearcher, OltDateRange, DateTimeOffset> Data
        {
            get
            {
                var now = DateTimeOffset.Now;
                var results = new TheoryData<FakeEntityDateRangeSearcher, OltDateRange, DateTimeOffset>();
                results.Add(new FakeEntityDateRangeSearcher(), new OltDateRange(), DateTimeOffset.MinValue.AddSeconds(1));
                results.Add(new FakeEntityDateRangeSearcher(now.Midnight(), now.Midnight().AddDays(3)), new OltDateRange(now.Midnight(), now.Midnight().AddDays(3)), now.Midnight().AddDays(3).AddSeconds(1));
                results.Add(new FakeEntityDateRangeSearcher(new OltDateRange(now, now.AddDays(4))), new OltDateRange(now, now.AddDays(4)), now.AddDays(4).AddSeconds(1));
                results.Add(new FakeEntityDateRangeSearcher(OltDateRange.Today), OltDateRange.Today, OltDateRange.Today.End.AddSeconds(1));
                return results;
            }
        }
    }
}