using FluentAssertions;
using FluentDate;
using FluentDateTimeOffset;
using OLT.Constants;
using OLT.Core;
using System;
using Xunit;

namespace OLT.Searchers.Tests
{
    public class DateRangeTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ModelTests(string becauseLabel, OltDateRange value, DateTimeOffset expectedStart, DateTimeOffset expectedEnd, string rangeLabel)
        {
            value.Should().BeEquivalentTo(new OltDateRange(expectedStart, expectedEnd, rangeLabel), becauseLabel);
        }


        public static TheoryData<string, OltDateRange, DateTimeOffset, DateTimeOffset, string> Data
        {
            get
            {

                var now = DateTimeOffset.Now;

                var results = new TheoryData<string, OltDateRange, DateTimeOffset, DateTimeOffset, string>();
                results.Add("1", new OltDateRange(), DateTimeOffset.MinValue, DateTimeOffset.MinValue, null);
                results.Add("2", new OltDateRange(now.AddDays(-5).AddHours(-5), now.AddDays(10)), now.AddDays(-5).AddHours(-5), now.AddDays(10), null);
                results.Add("Today", OltDateRange.Today, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.EndOfDay(), OltDateRangeLabels.Today);
                results.Add("Yesterday", OltDateRange.Yesterday, DateTimeOffset.Now.AddDays(-1).Midnight(), DateTimeOffset.Now.AddDays(-1).EndOfDay(), OltDateRangeLabels.Yesterday);
                results.Add("Tomorrow", OltDateRange.Tomorrow, DateTimeOffset.Now.AddDays(1).Midnight(), DateTimeOffset.Now.AddDays(1).EndOfDay(), OltDateRangeLabels.Tomorrow);
                results.Add("Last7Days", OltDateRange.Last7Days, DateTimeOffset.Now.AddDays(-7).Midnight(), DateTimeOffset.Now.EndOfDay(), OltDateRangeLabels.Last7Days);
                results.Add("Next7Days", OltDateRange.Next7Days, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.AddDays(7).EndOfDay(), OltDateRangeLabels.Next7Days);
                results.Add("ThisMonth", OltDateRange.ThisMonth, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.LastDayOfMonth().EndOfDay(), OltDateRangeLabels.ThisMonth);
                results.Add("LastMonth", OltDateRange.LastMonth, DateTimeOffset.Now.AddMonths(-1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(-1).LastDayOfMonth().EndOfDay(), OltDateRangeLabels.LastMonth);
                results.Add("NextMonth", OltDateRange.NextMonth, DateTimeOffset.Now.AddMonths(1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(1).LastDayOfMonth().EndOfDay(), OltDateRangeLabels.NextMonth);
                results.Add("MonthToDate", OltDateRange.MonthToDate, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.EndOfDay(), OltDateRangeLabels.MonthToDate);
                results.Add("ThisWeek", OltDateRange.ThisWeek, DateTimeOffset.Now.FirstDayOfWeek().Midnight(), DateTimeOffset.Now.LastDayOfWeek().EndOfDay(), OltDateRangeLabels.ThisWeek);
                results.Add("NextWeek", OltDateRange.NextWeek, DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).Midnight(), DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).LastDayOfWeek().EndOfDay(), OltDateRangeLabels.NextWeek);
                results.Add("YearToDate", OltDateRange.YearToDate, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.EndOfDay(), OltDateRangeLabels.YearToDate);
                results.Add("ThisYear", OltDateRange.ThisYear, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.LastDayOfYear().EndOfDay(), OltDateRangeLabels.ThisYear);
                results.Add("LastYear", OltDateRange.LastYear, DateTimeOffset.Now.AddYears(-1).FirstDayOfYear().Midnight(), DateTimeOffset.Now.AddYears(-1).LastDayOfYear().EndOfDay(), OltDateRangeLabels.LastYear);
                results.Add("QuarterToDate", OltDateRange.QuarterToDate, DateTimeOffset.Now.FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.EndOfDay(), OltDateRangeLabels.QuarterToDate);
                results.Add("PreviousQuarter", OltDateRange.PreviousQuarter, DateTimeOffset.Now.AddMonths(-3).FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.AddMonths(-3).LastDayOfQuarter().EndOfDay(), OltDateRangeLabels.PreviousQuarter);

                return results;
            }
        }
    }
}