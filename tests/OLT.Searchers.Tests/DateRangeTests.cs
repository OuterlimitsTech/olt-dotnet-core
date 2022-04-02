using FluentAssertions;
using FluentDate;
using FluentDateTimeOffset;
using OLT.Core;
using System;
using Xunit;

namespace OLT.Searchers.Tests
{
    public class DateRangeTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ModelTests(string label, OltDateRange value, DateTimeOffset expectedStart, DateTimeOffset expectedEnd)
        {
            value.Should().BeEquivalentTo(new OltDateRange(expectedStart, expectedEnd), label);
        }


        public static TheoryData<string, OltDateRange, DateTimeOffset, DateTimeOffset> Data
        {
            get
            {

                var now = DateTimeOffset.Now;

                var results = new TheoryData<string, OltDateRange, DateTimeOffset, DateTimeOffset>();
                results.Add("1", new OltDateRange(), DateTimeOffset.MinValue, DateTimeOffset.MinValue);
                results.Add("2", new OltDateRange(now.AddDays(-5).AddHours(-5), now.AddDays(10)), now.AddDays(-5).AddHours(-5), now.AddDays(10));
                results.Add("Today", OltDateRange.Today, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add("Yesterday", OltDateRange.Yesterday, DateTimeOffset.Now.AddDays(-1).Midnight(), DateTimeOffset.Now.AddDays(-1).EndOfDay());
                results.Add("Tomorrow", OltDateRange.Tomorrow, DateTimeOffset.Now.AddDays(1).Midnight(), DateTimeOffset.Now.AddDays(1).EndOfDay());
                results.Add("Last7Days", OltDateRange.Last7Days, DateTimeOffset.Now.AddDays(-7).Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add("Next7Days", OltDateRange.Next7Days, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.AddDays(7).EndOfDay());
                results.Add("ThisMonth", OltDateRange.ThisMonth, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.LastDayOfMonth().EndOfDay());
                results.Add("LastMonth", OltDateRange.LastMonth, DateTimeOffset.Now.AddMonths(-1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(-1).LastDayOfMonth().EndOfDay());
                results.Add("NextMonth", OltDateRange.NextMonth, DateTimeOffset.Now.AddMonths(1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(1).LastDayOfMonth().EndOfDay());
                results.Add("MonthToDate", OltDateRange.MonthToDate, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add("ThisWeek", OltDateRange.ThisWeek, DateTimeOffset.Now.FirstDayOfWeek().Midnight(), DateTimeOffset.Now.LastDayOfWeek().EndOfDay());
                results.Add("NextWeek", OltDateRange.NextWeek, DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).Midnight(), DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).LastDayOfWeek().EndOfDay());
                results.Add("YearToDate", OltDateRange.YearToDate, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add("ThisYear", OltDateRange.ThisYear, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.LastDayOfYear().EndOfDay());
                results.Add("LastYear", OltDateRange.LastYear, DateTimeOffset.Now.AddYears(-1).FirstDayOfYear().Midnight(), DateTimeOffset.Now.AddYears(-1).LastDayOfYear().EndOfDay());
                results.Add("QuarterToDate", OltDateRange.QuarterToDate, DateTimeOffset.Now.FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add("PreviousQuarter", OltDateRange.PreviousQuarter, DateTimeOffset.Now.AddMonths(-3).FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.AddMonths(-3).LastDayOfQuarter().EndOfDay());

                return results;
            }
        }
    }
}