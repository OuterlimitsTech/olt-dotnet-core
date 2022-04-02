using FluentAssertions;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public class MultipleFilterTests
    {

        [Fact]
        public void FilterTests()
        {
            var key1 = "first-name";
            var key2 = "last-name";
            var list = TestHelper.ValueList(10);
            var intValues = list.Select(s => s.Value).ToList();
            var expected = FakeEntity.FakerData(intValues);

            var seedRecs = FakeEntity.FakerList(5, intValues);
            seedRecs.Add(expected);
            seedRecs.AddRange(FakeEntity.FakerList(5, intValues));

            var recs = TestHelper.BuildRandomList(seedRecs, intValues, 1000, 3, 9);
            recs[3].FirstName = expected.FirstName;
            recs.Last().LastName = expected.LastName;

            var dict = new Dictionary<string, string>();
            dict.Add(key1, expected.FirstName);
            dict.Add(key2, expected.LastName);
            var parameters = new OltGenericParameter(dict);

            var filters = new List<IOltGenericFilter<FakeEntity>>();
            filters.Add(new OltFilterString<FakeEntity>(new OltFilterTemplateString(key1, "First Name"), new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.FirstName)));
            filters.Add(new OltFilterString<FakeEntity>(new OltFilterTemplateString(key2, "Last Name"), new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.LastName)));

            var searcher = new FakeEntityGenericFilterSearcher(parameters, filters);
            var results = searcher.BuildQueryable(recs.AsQueryable()).ToList();

            results.Should().HaveCount(1);
            results.First().Should().BeEquivalentTo(expected);

            var queryable = recs.AsQueryable();
            filters.ForEach(filter => queryable = filter.BuildQueryable(queryable));
            results = queryable.ToList();
            results.Should().HaveCount(1);
            results.First().Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void EmptyFilterTests()
        {
            var key1 = "first-name";
            var key2 = "last-name";
            var list = TestHelper.ValueList(10);
            var intValues = list.Select(s => s.Value).ToList();
            

            var seedRecs = FakeEntity.FakerList(5, intValues);
            seedRecs.AddRange(FakeEntity.FakerList(5, intValues, true));            
            seedRecs.AddRange(FakeEntity.FakerList(5, intValues));
            seedRecs.AddRange(FakeEntity.FakerList(5, intValues, true));

            var recs = TestHelper.BuildRandomList(seedRecs, intValues, 1000, 3, 9);
            var random = recs.Where(p => p.DeletedOn == null).OrderBy(p => Guid.NewGuid()).First();

            var dict = new Dictionary<string, string>();
            dict.Add(key1, random.FirstName);
            dict.Add(key2, random.LastName);
            var parameters = new OltGenericParameter(dict);

            var filters = new List<IOltGenericFilter<FakeEntity>>();

            var searcher = new FakeEntityGenericFilterSearcher(parameters, filters);
            var results = searcher.BuildQueryable(recs.AsQueryable()).ToList();

            var expected = recs.Where(p => p.DeletedOn == null);

            results.Should().HaveCount(expected.Count());
            results.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void NullParameterTests()
        {
            var key1 = "first-name";
            var key2 = "last-name";
            var list = TestHelper.ValueList(10);
            var intValues = list.Select(s => s.Value).ToList();


            var seedRecs = FakeEntity.FakerList(5, intValues);
            var recs = TestHelper.BuildRandomList(seedRecs, intValues, 1000, 3, 9);
            

            var dict = new Dictionary<string, string>();
            dict.Add(key1, null);            
            var parameters = new OltGenericParameter(dict);

            var filters = new List<IOltGenericFilter<FakeEntity>>();
            filters.Add(new OltFilterString<FakeEntity>(new OltFilterTemplateString(key1, "First Name"), new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.FirstName)));
            filters.Add(new OltFilterString<FakeEntity>(new OltFilterTemplateString(key2, "Last Name"), new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.LastName)));

            
            var searcher = new FakeEntityGenericFilterSearcher(parameters, filters);
            var results = searcher.BuildQueryable(recs.AsQueryable()).ToList();

            results.Should().HaveCount(recs.Count);

            var queryable = recs.AsQueryable();
            filters.ForEach(filter => queryable = filter.BuildQueryable(queryable));
            results = queryable.ToList();
            results.Should().HaveCount(recs.Count);

        }
    }
}
