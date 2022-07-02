using FluentAssertions;
using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public class StringFilterTests : BaseFilterTests
    {
        [Theory]
        [InlineData(true, "Test", "Test", true)]
        [InlineData(false, "Test", "Test", true)]
        [InlineData(false, "", null, false)]
        [InlineData(false, " ", null, false)]
        [InlineData(false, null, null, false)]
        public void String(bool hidden, string value, string expected, bool parseable)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var template = new OltFilterTemplateString(key, label, hidden);
            var filter = new OltFilterString<FakeEntity>(template, new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.FirstName));
            Assert.Null(template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.String, key, label, hidden, false);
            Assert.False(template.Parse(null));
            Assert.Null(template.Formatted());
            filter.Key.Should().BeEquivalentTo(key);
            Assert.Equal(parseable, template.Parse(TestHelper.BuildGenericParameter(key, value)));
            template.Value.Should().BeEquivalentTo(expected);            
            template.Formatted().Should().BeEquivalentTo(expected);
        }

    }
}
