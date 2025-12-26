using AwesomeAssertions;
using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System.Linq;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{

    public class SelectListFilterTests : BaseFilterTests
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
            var template = new OltFilterTemplateSelectList(key, label, listValues.Last(), listValues, hidden);
            var filter = new OltFilterSelect<FakeEntity>(template, p => p.SelectValue);
            template.ValueList.Should().BeEquivalentTo(listValues);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.SelectList, key, label, hidden, true);
        }

        [Fact]
        public void ParseTest()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectList(key, label, listValues.Last(), listValues);

            Assert.Equal(listValues.Last().Value, template.Value);

            Assert.True(template.Parse(null));
            Assert.Equal(listValues.Last().Value, template.Value);
            Assert.True(template.HasValue);

            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, listValues[4])));
            Assert.Equal(listValues[4].Value, template.Value);
            Assert.True(template.HasValue);

            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, random)));  //Select something not in the value list
            Assert.Equal(random, template.Value);
            Assert.True(template.HasValue);

            template.Value = int.MinValue;
            Assert.False(template.HasValue);
        }


        [Fact]
        public void FormattedTests()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectList(key, label, listValues.Last(), listValues);


            Assert.Equal(listValues.Last().Label, template.Formatted());
            template.Parse(TestHelper.BuildGenericParameter(key, listValues[4]));
            Assert.Equal(listValues[4].Label, template.Formatted());

            template.Parse(TestHelper.BuildGenericParameter(key, Faker.RandomNumber.Next(100, 150)));
            Assert.Equal(listValues.Last().Label, template.Formatted());


            var nonListDefaultValue = new OltValueListItem<int> { Label = Faker.Name.First(), Value = Faker.RandomNumber.Next(70, 80) };
            template = new OltFilterTemplateSelectList(key, label, nonListDefaultValue, listValues);

            template.Parse(TestHelper.BuildGenericParameter(key, Faker.RandomNumber.Next(100, 150)));

            Assert.Null(template.Formatted());

        }
    }
}
