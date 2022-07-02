using FluentAssertions;
using FluentDateTimeOffset;
using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System.Linq;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public class DateRangeFilterTests : BaseFilterTests
    {

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DateRange(bool hidden)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var ranges = TestHelper.DateRangeList();
            var template = new OltFilterTemplateDateRange(key, label, ranges.Last().Value, ranges, hidden);
            var filter = new OltFilterDateRange<FakeEntity>(template, new FakeEntityDateRangeSearcher());
            template.ValueList.Should().BeEquivalentTo(ranges);

            filter.Key.Should().BeEquivalentTo(key);
            filter.Key.Should().BeEquivalentTo(template.Key);            
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.DateRange, key, label, hidden, true);

        }

        [Fact]
        public void ParseTest()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var ranges = TestHelper.DateRangeList();
            var template = new OltFilterTemplateDateRange(key, label, ranges.Last().Value, ranges);

            Assert.Equal(ranges.Last()?.Value, template.Value);
            Assert.True(template.Parse(null));
            Assert.Equal(ranges.Last()?.Value, template.Value);


            var selected = ranges[2].Value;
            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, selected)));
            Assert.Equal(selected.Start.Midnight(), template.Value.Start);
            Assert.Equal(selected.End.Midnight(), template.Value.End);


            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, OltDateRange.NextWeek)));  //Select something not in the value list
            Assert.Equal(OltDateRange.NextWeek.Start, template.Value.Start);
            Assert.Equal(OltDateRange.NextWeek.End.Midnight(), template.Value.End);
            Assert.True(template.HasValue);

            template = new OltFilterTemplateDateRange(key, label, OltDateRange.LastYear, ranges);
            Assert.Equal(OltDateRange.LastYear.Start, template.Value.Start);
            Assert.Equal(OltDateRange.LastYear.End, template.Value.End);
            Assert.True(template.Parse(null));
            Assert.True(template.HasValue);
            Assert.Equal(OltDateRange.LastYear.Start, template.Value.Start);
            Assert.Equal(OltDateRange.LastYear.End, template.Value.End);


            template.Value = null;
            Assert.False(template.HasValue);
        }


        [Fact]
        public void FormattedTests()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var ranges = TestHelper.DateRangeList();
            var template = new OltFilterTemplateDateRange(key, label, ranges.Last().Value, ranges);



            var expectedFormat = $"{ranges.Last().Value?.Start.LocalDateTime:M/d/yyyy} to {ranges.Last().Value?.End.LocalDateTime:M/d/yyyy}";
            Assert.Equal(expectedFormat, template.Formatted());

            var selected = ranges[2].Value;
            template.Parse(TestHelper.BuildGenericParameter(key, selected));
            expectedFormat = $"{selected.Start.Midnight().LocalDateTime:M/d/yyyy} to {selected.End.Midnight().LocalDateTime:M/d/yyyy}";
            Assert.Equal(expectedFormat, template.Formatted());


            template.Value = null;
            Assert.Equal(" to ", template.Formatted());

        }
    }
}
