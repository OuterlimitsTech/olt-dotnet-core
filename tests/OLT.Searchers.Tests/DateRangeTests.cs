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
            var formatString = "u";
            value.ToString(formatString).Should().BeEquivalentTo($"{expectedStart.ToLocalTime().ToString(formatString)} to {expectedEnd.ToLocalTime().ToString(formatString)}");
        }


        public static TheoryData<OltDateRange, DateTimeOffset, DateTimeOffset> Data
        {
            get
            {
                DateTimeOffset ToEndValue(DateTimeOffset value)
                {
                    return value.Midnight().AddDays(1).AddMilliseconds(-1);
                }

                var now = DateTimeOffset.Now;

                var results = new TheoryData<OltDateRange, DateTimeOffset, DateTimeOffset>();
                results.Add(new OltDateRange(), DateTimeOffset.MinValue, DateTimeOffset.MinValue);
                results.Add(new OltDateRange(now.AddDays(-5).AddHours(-5), now.AddDays(10)), now.AddDays(-5).AddHours(-5), now.AddDays(10));
                results.Add(OltDateRange.Today, DateTimeOffset.Now.Midnight(), ToEndValue(DateTimeOffset.Now));
                results.Add(OltDateRange.Yesterday, DateTimeOffset.Now.PreviousDay().Midnight(), ToEndValue(DateTimeOffset.Now.PreviousDay()));
                results.Add(OltDateRange.Last7Days, DateTimeOffset.Now.AddDays(-7).Midnight(), ToEndValue(DateTimeOffset.Now));
                results.Add(OltDateRange.Next7Days, DateTimeOffset.Now.Midnight(), ToEndValue(DateTimeOffset.Now.AddDays(7)));
                results.Add(OltDateRange.ThisMonth, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), ToEndValue(DateTimeOffset.Now.LastDayOfMonth()));
                results.Add(OltDateRange.LastMonth, DateTimeOffset.Now.PreviousMonth().FirstDayOfMonth().Midnight(), ToEndValue(DateTimeOffset.Now.PreviousMonth().LastDayOfMonth()));
                results.Add(OltDateRange.MonthToDate, DateTimeOffset.Now.FirstDayOfMonth().Midnight(), ToEndValue(DateTimeOffset.Now));
                results.Add(OltDateRange.ThisWeek, DateTimeOffset.Now.FirstDayOfWeek().Midnight(), ToEndValue(DateTimeOffset.Now.LastDayOfWeek()));
                results.Add(OltDateRange.NextWeek, DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).Midnight(), ToEndValue(DateTimeOffset.Now.FirstDayOfWeek().AddDays(7).LastDayOfWeek()));

                return results;
            }
        }
    }
}