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


        public static OltDateRange Today
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, seed.EndOfDay());
            }
        }

        public static OltDateRange Yesterday
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousDay().Midnight();
                return new OltDateRange(seed, seed.EndOfDay());
            }
        }


        public static OltDateRange Tomorrow
        {
            get
            {
                var seed = DateTimeOffset.Now.NextDay().Midnight();
                return new OltDateRange(seed, seed.EndOfDay());
            }
        }
        
        public static OltDateRange Last7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(7.Days().Ago().Midnight(), seed.EndOfDay());
            }
        }

        public static OltDateRange Next7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, 7.Days().FromNow().EndOfDay());
            }
        }

        public static OltDateRange ThisMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfMonth().Midnight();
                return new OltDateRange(seed, seed.LastDayOfMonth().EndOfDay());
            }
        }

        public static OltDateRange LastMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousMonth().FirstDayOfMonth().Midnight();
                return new OltDateRange(seed, seed.LastDayOfMonth().EndOfDay());
            }
        }

        public static OltDateRange NextMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.NextMonth().FirstDayOfMonth().Midnight();
                return new OltDateRange(seed, seed.LastDayOfMonth().EndOfDay());
            }
        }

        public static OltDateRange MonthToDate
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfMonth().Midnight(), seed.EndOfDay());
            }
        }

        public static OltDateRange ThisWeek
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfWeek().Midnight(), seed.LastDayOfWeek().EndOfDay());
            }
        }

        public static OltDateRange NextWeek
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfWeek().Midnight() + 1.Weeks();
                return new OltDateRange(seed, seed.LastDayOfWeek().EndOfDay());
            }
        }

        public static OltDateRange YearToDate
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfYear().Midnight(), seed.EndOfDay());
            }
        }

        public static OltDateRange ThisYear
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfYear().Midnight();
                return new OltDateRange(seed, seed.LastDayOfYear().EndOfDay());
            }
        }

        public static OltDateRange LastYear
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousYear().FirstDayOfYear().Midnight();
                return new OltDateRange(seed, seed.LastDayOfYear().EndOfDay());
            }
        }

        public static OltDateRange QuarterToDate
        {
            get
            {
                var seed = DateTimeOffset.Now.FirstDayOfQuarter().Midnight();
                return new OltDateRange(seed, seed.EndOfDay());
            }
        }

        public static OltDateRange PreviousQuarter
        {
            get
            {
                var seed = DateTimeOffset.Now.AddMonths(-3).FirstDayOfQuarter().Midnight();
                return new OltDateRange(seed, seed.LastDayOfQuarter().EndOfDay());
            }
        }

        
    }
}
