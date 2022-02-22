using OLT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class AttributeExtensionsTests
    {

        public enum TestAttributeEnum1
        {
            [Description("Test Value 1")]
            Value1,

            [CustomTest("value2")]
            Value2,

            [CustomTest("value3a")]
            [CustomTest("value3b")]
            [CustomTest("value3c")]
            Value3,

            Value4,

        }


        [Theory]
        [InlineData("Test Value 1", TestAttributeEnum1.Value1)]
        [InlineData(null, TestAttributeEnum1.Value2)]
        [InlineData(null, null)]
        public void GetAttributeInstance(string expected, TestAttributeEnum1? @enum)
        {
            Assert.Equal(expected, OltAttributeExtensions.GetAttributeInstance<DescriptionAttribute>(@enum)?.Description);
            if (@enum.HasValue)
            {
                Assert.Equal(expected, OltAttributeExtensions.GetAttributeInstance<DescriptionAttribute, TestAttributeEnum1>(@enum.Value)?.Description);
            }
            
        }

        [Theory]
        [InlineData("Test Value 1", TestAttributeEnum1.Value1)]
        [InlineData("Value2", TestAttributeEnum1.Value2)]
        public void GetDescription1(string expected, TestAttributeEnum1 value)
        {
            Assert.Equal(expected, OltAttributeExtensions.GetDescription(value));
        }


        [Fact]
        public void GetAttributeInstanceInvalid()
        {
            Assert.Null(OltAttributeExtensions.GetAttributeInstance<DescriptionAttribute>(null)?.Description);
            Assert.Throws<InvalidOperationException>(() => OltAttributeExtensions.GetAttributeInstance<CustomTestAttribute>(TestAttributeEnum1.Value3));
        }

   

        [Fact]
        public void GetDescriptionNull()
        {
            Assert.Null(OltAttributeExtensions.GetDescription(null));
        }

        [Theory]
        [InlineData("Test Class Value1", "Value1", false)]
        [InlineData("Test Class Value1", "Value1", true)]
        [InlineData("Base Class Value3", "Value3", false)]
        [InlineData("Base Class Value3", "Value3", true)]
        [InlineData(null, "FooBarInvalid", false)]
        [InlineData(null, "FooBarInvalid", true)]
        [InlineData(null, "Value2", false)]
        [InlineData(null, "Value2", true)]
        [InlineData(null, "Value4", false)]
        [InlineData(null, "Value4", true)]
        public void GetAttributeInstanceByProperty(string expected, string propertyName, bool inherit)
        {
            var obj = new TestAttributeClass();
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            var value = OltAttributeExtensions.GetAttributeInstance<DescriptionAttribute>(propertyInfo, inherit);
            Assert.Equal(expected, value?.Description);
        }

        [Fact]
        public void GetAttributeInstanceByPropertyInvalid()
        {
            var obj = new TestAttributeClass();
            PropertyInfo propertyInfo = obj.GetType().GetProperty("Value1");
            Assert.Throws<InvalidOperationException>(() => OltAttributeExtensions.GetAttributeInstance<CustomTestAttribute>(propertyInfo));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CustomTestAttributeTest(bool inherit)
        {
            var obj = new TestAttributeClass();

            var propertyInfo = obj.GetType().GetProperty(nameof(obj.Value2));
            var values = OltAttributeExtensions.GetAttributeInstances<CustomTestAttribute>(propertyInfo, inherit);
            Assert.True(values.Count == 0);

            propertyInfo = obj.GetType().GetProperty(nameof(obj.Value1));
            values = OltAttributeExtensions.GetAttributeInstances<CustomTestAttribute>(propertyInfo, inherit);
            Assert.True(values.Count == 2);

            propertyInfo = obj.GetType().GetProperty(Faker.Name.First());
            values = OltAttributeExtensions.GetAttributeInstances<CustomTestAttribute>(propertyInfo, inherit);
            Assert.Null(values);
        }


        [Theory]
        [InlineData(TestAttributeEnum1.Value1, "Test Value 1")]
        [InlineData(TestAttributeEnum1.Value2, "Value2")]
        public void FromDescription(TestAttributeEnum1 expected, string value)
        {
            Assert.Equal(expected, OltAttributeExtensions.FromDescription<TestAttributeEnum1>(value));
        }

        [Fact]
        public void FromDescriptionInvalid()
        {
            Assert.Throws<ArgumentException>(() => OltAttributeExtensions.FromDescription<TestAttributeEnum1>(Faker.Name.First()));
            Assert.Throws<ArgumentNullException>(() => OltAttributeExtensions.FromDescription<TestAttributeEnum1>(null));
        }
    }
}
