using OLT.Constants;
using OLT.Core;
using OLT.Searchers.Tests.Assets;
using System;
using System.Linq;
using Xunit;

namespace OLT.Searchers.Tests
{
    public class GenericFilterTests
    {
        [Theory]
        [InlineData("Test", "Test", true)]
        [InlineData("", null, false)]
        [InlineData(" ", null, false)]
        [InlineData(null, null, false)]
        public void String(string value, string expected, bool parseable)
        {
            var entities = FakeEntity.FakerList(10).AsQueryable();
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var template = new OltFilterTemplateString(key, label, false);
            var expr = new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.FirstName);
            var genericParameter = TestHelper.BuildGenericParameter(key, value);
            var filter = new OltGenericFilter<FakeEntity, string>(template, expr);
            Assert.Equal(parseable, filter.HasValue(genericParameter));
            Assert.Equal(expected, filter.GetValue(genericParameter));

            if (parseable)
            {
                Assert.Empty(filter.BuildQueryable(entities).ToList());
            }
            else
            {
                Assert.NotEmpty(filter.BuildQueryable(entities).ToList());
            }
        }

       
    }
}