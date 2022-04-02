using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class GenericParameterTests
    {

        [Fact]
        public void GetValueTests()
        {
            var numVal = Faker.RandomNumber.Next();
            var values = new Dictionary<string, string>
            {
                {"Key1", Faker.Name.First()},
                {"Key2", null },
                {"Key3", numVal.ToString() }
            };


            var result = new OltGenericParameter(values);
            result.Values.Should().BeEquivalentTo(values);

            Assert.Equal(values["Key1"], result.GetValue<string>("Key1"));
            Assert.Equal(values["Key2"], result.GetValue<string>("Key2"));

            Assert.Equal(values["Key1"], result.GetValue("Key1"));
            Assert.Equal(values["Key2"], result.GetValue("Key2"));

            Assert.Equal(0, result.GetValue<int>("Key1"));
            Assert.Equal(-10, result.GetValue<int>("Key1", -10));

            Assert.Equal(0, result.GetValue<int>("Key2"));
            Assert.Equal(-10, result.GetValue<int>("Key2", -10));

            Assert.Equal(numVal, result.GetValue<int>("Key3"));
            Assert.Equal(numVal, result.GetValue<int>("Key3", -100));

        }


        [Theory]
        [InlineData("ABC123", "ABC123", true)]
        [InlineData(null, null, false)]
        [InlineData("", null, false)]
        [InlineData(" ", " ", true)]
        public void OltGenericParameterParserTests(string value, string expected, bool parsed)
        {            
            var values = new Dictionary<string, string>
            {
                {"FirstName", Faker.Name.First()},
                {nameof(ParameterStringParser), value},
                {"MiddleName", null},
                {"NumValue", Faker.RandomNumber.Next().ToString()},
            };

            var parameter = new OltGenericParameter(values);
            var parser = new ParameterStringParser();
            Assert.Equal(parsed, parser.Parse(parameter));
            Assert.Equal(expected, parser.Value);
        }
    }
}