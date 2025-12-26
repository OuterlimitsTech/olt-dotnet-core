using AwesomeAssertions;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Searchers.Tests.FilterTests
{
    public abstract class BaseFilterTests
    {
        protected void GeneralTemplateTests(IOltGenericFilterTemplate filter, IOltFilterTemplate filterTemplate, string templateName, string key, string label, bool hidden, bool hasValue)
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
