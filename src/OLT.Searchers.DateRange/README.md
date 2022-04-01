[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## OltSearcherDateRange and OltDateRange model class to build a date range searchers/expressions (IQueryable)

_Utilizes [FluentDateTime](https://www.nuget.org/packages/FluentDateTime/) to build out OltDateRange defaults (i.e., OltDateRange.Today)_

```csharp
public class MyEntitySearcher : OltSearcherDateRange<MyEntity>
{
    public MyEntitySearcher() : base() { }

    public MyEntitySearcher(OltDateRange value) : base(value) { }

    public MyEntitySearcher(DateTimeOffset start, DateTimeOffset end) : base(start, end) { }

    public override IQueryable<MyEntity> BuildQueryable(IQueryable<MyEntity> queryable)
    {
        return queryable.Where(p => p.SomeDate >= Value.Start && p.SomeDate < QueryEnd);
    }
}


// Example Searchers
MyEntitySearcher searcher;
searcher = new MyEntitySearcher(new OltDateRange(DateTimeOffset.Now.AddMonths(-3), DateTimeOffset.Now));

searcher = new MyEntitySearcher(OltDateRange.Today);
searcher = new MyEntitySearcher(OltDateRange.Yesterday);
searcher = new MyEntitySearcher(OltDateRange.Last7Days);
searcher = new MyEntitySearcher(OltDateRange.Next7Days);
searcher = new MyEntitySearcher(OltDateRange.ThisMonth);
searcher = new MyEntitySearcher(OltDateRange.LastMonth);
searcher = new MyEntitySearcher(OltDateRange.MonthToDate);
searcher = new MyEntitySearcher(OltDateRange.ThisWeek);
searcher = new MyEntitySearcher(OltDateRange.NextWeek);

//Midnight() is an extension provided by FluentDateTime.  See The package for all the extensions
searcher = new MyEntitySearcher(DateTimeOffset.Now.Midnight(), DateTimeOffset.Now.AddDays(3).Midnight());

//TODO: QuarterToDate, PreviousQuarter, YTD, LastYear, NextMonth, ThisYear, Tomorrow

```
