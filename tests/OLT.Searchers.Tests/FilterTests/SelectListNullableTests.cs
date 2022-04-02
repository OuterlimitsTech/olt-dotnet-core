using FluentAssertions;
using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public class SelectListNullableFilterTests : BaseFilterTests
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
            var template = new OltFilterTemplateSelectListNullable(key, label, listValues, hidden);
            var filter = new OltFilterSelectOptional<FakeEntity>(template, p => p.SelectValue);
            template.ValueList.Should().BeEquivalentTo(listValues);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.SelectList, key, label, hidden, false);
        }

        [Fact]
        public void ParseTest()
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectListNullable(key, label, listValues);

            Assert.Null(template.Value);

            Assert.False(template.Parse(null));
            Assert.Null(template.Value);

            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, listValues[4])));
            Assert.Equal(listValues[4].Value, template.Value);

            Assert.True(template.Parse(TestHelper.BuildGenericParameter(key, random)));  //Select something not in the value list
            Assert.Equal(random, template.Value);



        }

        [Fact]
        public void FormattedTests()
        {
            var random = Faker.RandomNumber.Next(1000, 10000);
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectListNullable(key, label, listValues);


            Assert.Null(template.Formatted());
            template.Parse(null);
            Assert.Null(template.Formatted());


            template.Parse(TestHelper.BuildGenericParameter(key, listValues[4]));
            Assert.Equal(listValues[4].Label, template.Formatted());

            template.Parse(TestHelper.BuildGenericParameter(key, random));
            Assert.Null(template.Formatted());

        }
    }
}
