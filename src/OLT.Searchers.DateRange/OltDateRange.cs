using FluentDate;
using FluentDateTimeOffset;
using System;

namespace OLT.Core
{

    public class OltDateRange
    {
        public OltDateRange()
        {

        }

        public OltDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public string ToString(string format)
        {
            return $"{Start.ToLocalTime().ToString(format)} to {End.ToLocalTime().ToString(format)}";
        }

        private static DateTimeOffset ToEnd(DateTimeOffset value) => value.NextDay().AddMilliseconds(-1);

        public static OltDateRange Today
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, ToEnd(seed));
            }
        }

        public static OltDateRange Yesterday
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousDay().Midnight();
                return new OltDateRange(seed, ToEnd(seed));
            }
        }

        public static OltDateRange Last7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(7.Days().Ago().Midnight(), ToEnd(seed));
            }
        }

        public static OltDateRange Next7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, ToEnd(7.Days().FromNow().Midnight()));
            }
        }

        public static OltDateRange ThisMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfMonth().Midnight();
                return new OltDateRange(seed, ToEnd(seed.LastDayOfMonth()));
            }
        }

        public static OltDateRange LastMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousMonth().FirstDayOfMonth().Midnight();
                return new OltDateRange(seed, ToEnd(seed.LastDayOfMonth()));
            }
        }


        public static OltDateRange MonthToDate
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed.FirstDayOfMonth(), ToEnd(seed));
            }
        }

        public static OltDateRange ThisWeek
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfWeek().Midnight();
                return new OltDateRange(seed, ToEnd(seed.LastDayOfWeek()));
            }
        }

        public static OltDateRange NextWeek
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfWeek().Midnight() + 1.Weeks();
                return new OltDateRange(seed, ToEnd(seed.LastDayOfWeek()));
            }
        }

        //TODO: QuarterToDate, PreviousQuarter, YTD, LastYear, NextMonth, ThisYear, Tomorrow
    }
}
