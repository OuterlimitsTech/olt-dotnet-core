using FluentAssertions;
using FluentDate;
using FluentDateTimeOffset;
using System;
using Xunit;

namespace OLT.Core.Searchers.Tests
{
    public class DateRangeTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ModelTests(OltDateRange value, DateTimeOffset expectedStart, DateTimeOffset expectedEnd)
        {            
            value.Should().BeEquivalentTo(new OltDateRange(expectedStart, expectedEnd));
        }


        public static TheoryData<OltDateRange, DateTimeOffset, DateTimeOffset> Data
        {
            get
            {

                var now = DateTimeOffset.Now;

                var results = new TheoryData<OltDateRange, DateTimeOffset, DateTimeOffset>();
                results.Add(new OltDateRange(), DateTimeOffset.MinValue, DateTimeOffset.MinValue);
                results.Add(new OltDateRange(now.AddDays(-5).AddHours(-5), now.AddDays(10)), now.AddDays(-5).AddHours(-5), now.AddDays(10));
                results.Add(OltDateRange.Today, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add(OltDateRange.Yesterday, DateTimeOffset.Now.AddDays(-1).Midnight(), DateTimeOffset.Now.AddDays(-1).EndOfDay());
                results.Add(OltDateRange.Tomorrow, DateTimeOffset.Now.AddDays(1).Midnight(), DateTimeOffset.Now.AddDays(1).EndOfDay());
                results.Add(OltDateRange.Last7Days, DateTimeOffset.Now.AddDays(-7).Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add(OltDateRange.Next7Days, DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.AddDays(7).EndOfDay());
                results.Add(OltDateRange.ThisMonth, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.LastDayOfMonth().EndOfDay());
                results.Add(OltDateRange.LastMonth, DateTimeOffset.Now.AddMonths(-1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(-1).LastDayOfMonth().EndOfDay());
                results.Add(OltDateRange.NextMonth, DateTimeOffset.Now.AddMonths(1).FirstDayOfMonth().Midnight(), DateTimeOffset.Now.AddMonths(1).LastDayOfMonth().EndOfDay());
                results.Add(OltDateRange.MonthToDate, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add(OltDateRange.ThisWeek, DateTimeOffset.Now.FirstDayOfWeek().Midnight(), DateTimeOffset.Now.LastDayOfWeek().EndOfDay());
                results.Add(OltDateRange.NextWeek, DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).Midnight(), DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).LastDayOfWeek().EndOfDay());
                results.Add(OltDateRange.YearToDate, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add(OltDateRange.ThisYear, DateTimeOffset.Now.FirstDayOfYear().Midnight(), DateTimeOffset.Now.LastDayOfYear().EndOfDay());
                results.Add(OltDateRange.LastYear, DateTimeOffset.Now.AddYears(-1).FirstDayOfYear().Midnight(), DateTimeOffset.Now.AddYears(-1).LastDayOfYear().EndOfDay());
                results.Add(OltDateRange.QuarterToDate, DateTimeOffset.Now.FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.EndOfDay());
                results.Add(OltDateRange.PreviousQuarter, DateTimeOffset.Now.AddMonths(-3).FirstDayOfQuarter().Midnight(), DateTimeOffset.Now.AddMonths(-3).LastDayOfQuarter().EndOfDay());
                
                return results;
            }
        }
    }
}