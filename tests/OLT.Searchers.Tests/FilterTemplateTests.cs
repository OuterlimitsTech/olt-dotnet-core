﻿using FluentAssertions;
using OLT.Constants;
using OLT.Core;
using OLT.Core.Searchers.Tests.Assets;
using OLT.Searchers.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Searchers.Tests
{
    public class FilterTemplateTests
    {

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectListNullableTests(bool hidden)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectListNullable(key, label, listValues, hidden);
            var filter = new OltFilterSelectOptional<FakeEntity>(template, p => p.SelectValue);
            template.ValueList.Should().BeEquivalentTo(listValues);
            Assert.Null(template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.SelectList, key, label, hidden, false);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectList(bool hidden)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var listValues = TestHelper.ValueList(10);
            var template = new OltFilterTemplateSelectList(key, label, listValues.Last(), listValues, hidden);
            var filter = new OltFilterSelect<FakeEntity>(template, p => p.SelectValue);
            template.ValueList.Should().BeEquivalentTo(listValues);            
            Assert.Equal(listValues.Last()?.Value, template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.SelectList, key, label, hidden, true);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MultiSelect(bool hidden)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void String(bool hidden)
        {
            var random = Faker.RandomNumber.Next();
            var key = $"key_{random}";
            var label = $"label_{random}";
            var template = new OltFilterTemplateString(key, label, hidden);
            var filter = new OltFilterString<FakeEntity>(template, new OltEntityExpressionStringStartsWith<FakeEntity>(p => p.FirstName)); 
            Assert.Null(template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.String, key, label, hidden, false);
        }

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
            Assert.Equal(ranges.Last()?.Value, template.Value);
            GeneralTemplateTests(filter, template, OltGenericParameterTemplates.DateRange, key, label, hidden, true);
        }

        private void GeneralTemplateTests(IOltGenericFilterTemplate filter, IOltFilterTemplate filterTemplate, string templateName, string key, string label, bool hidden, bool hasValue)
        {
            Assert.Equal(key, filter.FilterTemplate.Key);
            Assert.Equal(label, filter.FilterTemplate.Label);
            Assert.Equal(templateName, filter.FilterTemplate.TemplateName);
            Assert.Equal(hidden, filter.FilterTemplate.Hidden);
            Assert.Equal(hasValue, filter.FilterTemplate.HasValue);
            filter.FilterTemplate.Should().BeEquivalentTo(filterTemplate);
        }
    }
}