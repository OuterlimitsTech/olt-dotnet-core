using FluentDate;
using FluentDateTimeOffset;
using OLT.Constants;
using System;

namespace OLT.Core
{
    /// <summary>
    /// Defines a Date Range
    /// </summary>
    public class OltDateRange
    {
        public OltDateRange()
        {

        }

        public OltDateRange(DateTimeOffset start, DateTimeOffset end, string? label = null)
        {
            Start = start;
            End = end;
            Label = label;
        }


        /// <summary>
        /// Used to describe the date range (Optional)
        /// </summary>
        public string? Label { get; set; }


        /// <summary>
        /// Start of Date Range
        /// </summary>
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// End of Date Range
        /// </summary>
        public DateTimeOffset End { get; set; }


        /// <summary>
        /// From midnight to end of day using <see cref="DateTimeOffsetExtensions.Midnight(DateTimeOffset)"/> and <see cref="DateTimeOffsetExtensions.EndOfDay(DateTimeOffset)"/>
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange Today
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, seed.EndOfDay(), OltDateRangeLabels.Today);
            }
        }

        /// <summary>
        /// From midnight to end of day for <see cref="DateTimeOffsetExtensions.PreviousDay(DateTimeOffset)"/>
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange Yesterday
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousDay().Midnight();
                return new OltDateRange(seed, seed.EndOfDay(), OltDateRangeLabels.Yesterday);
            }
        }

        /// <summary>
        /// From midnight to end of day for <see cref="DateTimeOffsetExtensions.NextDay(DateTimeOffset)"/>
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange Tomorrow
        {
            get
            {
                var seed = DateTimeOffset.Now.NextDay().Midnight();
                return new OltDateRange(seed, seed.EndOfDay(), OltDateRangeLabels.Tomorrow);
            }
        }

        /// <summary>
        /// From midnight 7 days ago using <see cref="NumberExtensions.Days(int)"/> to end of day for <see cref="DateTimeOffsetExtensions.EndOfDay(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange Last7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(7.Days().Ago().Midnight(), seed.EndOfDay(), OltDateRangeLabels.Last7Days);
            }
        }

        /// <summary>
        /// Build a date range from midnight from <see cref="DateTimeOffset.Now"/> to end of day from now 7 days <see cref="NumberExtensions.Days(int)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange Next7Days
        {
            get
            {
                var seed = DateTimeOffset.Now.Midnight();
                return new OltDateRange(seed, 7.Days().FromNow().EndOfDay(), OltDateRangeLabels.Next7Days);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfMonth(DateTimeOffset)"/> to end of day for <see cref="DateTimeOffsetExtensions.LastDayOfMonth(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> 
        /// </remarks>
        public static OltDateRange ThisMonth
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfMonth().Midnight(), seed.LastDayOfMonth().EndOfDay(), OltDateRangeLabels.ThisMonth);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfMonth(DateTimeOffset)"/> to end of day for <see cref="DateTimeOffsetExtensions.LastDayOfMonth(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> with <see cref="DateTimeOffsetExtensions.PreviousMonth(DateTimeOffset)"/>
        /// </remarks>
        public static OltDateRange LastMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousMonth();
                return new OltDateRange(seed.FirstDayOfMonth().Midnight(), seed.LastDayOfMonth().EndOfDay(), OltDateRangeLabels.LastMonth);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfMonth(DateTimeOffset)"/> to end of day for <see cref="DateTimeOffsetExtensions.LastDayOfMonth(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> with <see cref="DateTimeOffsetExtensions.NextMonth(DateTimeOffset)"/>
        /// </remarks>
        public static OltDateRange NextMonth
        {
            get
            {
                var seed = DateTimeOffset.Now.NextMonth();
                return new OltDateRange(seed.FirstDayOfMonth().Midnight(), seed.LastDayOfMonth().EndOfDay(), OltDateRangeLabels.NextMonth);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfMonth(DateTimeOffset)"/> to end of day of seed
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/>
        /// </remarks>
        public static OltDateRange MonthToDate
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfMonth().Midnight(), seed.EndOfDay(), OltDateRangeLabels.MonthToDate);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfWeek(DateTimeOffset)"/> to end of day for <see cref="DateTimeOffsetExtensions.LastDayOfWeek(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/>
        /// </remarks>
        public static OltDateRange ThisWeek
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfWeek().Midnight(), seed.LastDayOfWeek().EndOfDay(), OltDateRangeLabels.ThisWeek);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfWeek(DateTimeOffset)"/> to end of day for <see cref="DateTimeOffsetExtensions.LastDayOfWeek(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> + 1 week <see cref="NumberExtensions.Weeks(int)"/> 
        /// </remarks>
        public static OltDateRange NextWeek
        {
            get
            {
                var seed = DateTimeOffset.Now + 1.Weeks();
                return new OltDateRange(seed.FirstDayOfWeek().Midnight(), seed.LastDayOfWeek().EndOfDay(), OltDateRangeLabels.NextWeek);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfYear(DateTimeOffset)"/> to end of day of seed
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/>
        /// </remarks>
        public static OltDateRange YearToDate
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfYear().Midnight(), seed.EndOfDay(), OltDateRangeLabels.YearToDate);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfYear(DateTimeOffset)"/> to end of day <see cref="DateTimeOffsetExtensions.LastDayOfYear(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/>
        /// </remarks>
        public static OltDateRange ThisYear
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfYear().Midnight(), seed.LastDayOfYear().EndOfDay(), OltDateRangeLabels.ThisYear);
            }
        }


        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfYear(DateTimeOffset)"/> to end of day <see cref="DateTimeOffsetExtensions.LastDayOfYear(DateTimeOffset)"/> 
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> and <see cref="DateTimeOffsetExtensions.PreviousYear(DateTimeOffset)"/>
        /// </remarks>
        public static OltDateRange LastYear
        {
            get
            {
                var seed = DateTimeOffset.Now.PreviousYear();
                return new OltDateRange(seed.FirstDayOfYear().Midnight(), seed.LastDayOfYear().EndOfDay(), OltDateRangeLabels.LastYear);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfQuarter(DateTimeOffset)"/> to end of day of seed
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/>
        /// </remarks>
        public static OltDateRange QuarterToDate
        {
            get
            {
                var seed = DateTimeOffset.Now;
                return new OltDateRange(seed.FirstDayOfQuarter().Midnight(), seed.EndOfDay(), OltDateRangeLabels.QuarterToDate);
            }
        }

        /// <summary>
        /// From midnight of <see cref="DateTimeOffsetExtensions.FirstDayOfQuarter(DateTimeOffset)"/> to end of day of seed <see cref="DateTimeOffsetExtensions.LastDayOfQuarter(DateTimeOffset)"/>
        /// </summary>
        /// <remarks>
        /// Seed of <see cref="DateTimeOffset.Now"/> subtracting 3 months using <see cref="DateTimeOffset.AddMonths(int)"/>
        /// </remarks>
        public static OltDateRange PreviousQuarter
        {
            get
            {
                var seed = DateTimeOffset.Now.AddMonths(-3);
                return new OltDateRange(seed.FirstDayOfQuarter().Midnight(), seed.LastDayOfQuarter().EndOfDay(), OltDateRangeLabels.PreviousQuarter);
            }
        }

        
    }
}
