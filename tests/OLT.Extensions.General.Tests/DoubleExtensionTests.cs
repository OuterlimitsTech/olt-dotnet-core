using System;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class DoubleExtensionTests
    {
        [Theory]
        [InlineData(34.559, 34.56)]
        [InlineData(34.554, 34.55)]
        [InlineData(34.5545, 34.55)]
        [InlineData(34.5549, 34.55)]
        [InlineData(34, 34)]
        public void ToDollars(double value, double expectedResult)
        {
            Assert.Equal(expectedResult, OltDoubleExtensions.ToDollars(value));
        }

        [Theory]
        [InlineData(34.559, 34.56)]
        [InlineData(34.554, 34.55)]
        [InlineData(34.5545, 34.55)]
        [InlineData(34.5549, 34.55)]
        [InlineData(34d, 34d)]
        [InlineData(null, null)]
        public void ToDollarsNullable(double? value, double? expectedResult)
        {
            Assert.Equal(expectedResult, OltDoubleExtensions.ToDollars(value));
        }

    }
}