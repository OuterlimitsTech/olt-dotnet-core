using AwesomeAssertions;
using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public class MultiSelectFilterTests : BaseFilterTests
    {


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void StandardTests(bool hidden)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateMultiSelectList(key, label, listValues, hidden);

            var filter = new OltFilterMultiSelect<FakeEntity>(template, p => p.SelectValue);
            template.ValueList.Should().BeEquivalentTo(listValues);
            Assert.Null(template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.MultiSelectList, key, label, hidden, false);

        }

        [Fact]
        public void ParseTest()
        {
            var random = Faker.RandomNumber.Next(1000, 100000);
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateMultiSelectList(key, label, listValues);

            Assert.Null(template.Value);
            Assert.False(template.Parse(null));
            Assert.Null(template.Value);
            Assert.False(template.HasValue);

            var expected = new List<OltValueListItem<int>> { listValues[1], listValues[4], listValues[7] };
            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, expected)));
            template.Value.Should().BeEquivalentTo(expected.Select(s => s.Value));
            Assert.True(template.HasValue);

            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, new OltValueListItem<int>(Faker.Name.First(), random))));  //Select something not in the value list
            Assert.Equal(new List<int> { random }, template.Value);
            Assert.True(template.HasValue);

            template.Value = null;
            Assert.False(template.HasValue);
        }

        [Fact]
        public void FormattedTests()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateMultiSelectList(key, label, listValues);


            Assert.Null(template.Formatted());
            template.Parse(null);
            Assert.Null(template.Formatted());

            var expected = new List<OltValueListItem<int>> { listValues[2], listValues[3], listValues[5] };
            var expectedString = string.Join(",", expected.Select(s => s.Label));
            template.Parse(TestHelper.BuildGenericParameter(key, expected));
            Assert.Equal(expectedString, template.Formatted());

            template.Parse(TestHelper.BuildGenericParameter(key, Faker.RandomNumber.Next(100, 150)));
            Assert.Equal("", template.Formatted());

        }
    }
}
