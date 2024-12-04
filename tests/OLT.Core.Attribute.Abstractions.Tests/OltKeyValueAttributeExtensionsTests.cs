namespace OLT.Core.Attribute.Abstractions.Tests
{
    public class OltKeyValueAttributeExtensionsTests
    {
        private enum TestEnum
        {
            [KeyValue("Key1", "Value1")]
            [KeyValue("Key2", "Value2")]
            ValueWithAttributes,

            ValueWithoutAttributes
        }

        [Fact]
        public void GetKeyValueAttributes_ShouldReturnAttributes_WhenEnumValueHasAttributes()
        {
            // Arrange
            var enumValue = TestEnum.ValueWithAttributes;

            // Act
            var attributes = enumValue.GetKeyValueAttributes();

            // Assert
            Assert.NotNull(attributes);
            Assert.Equal(2, attributes.Count);
            Assert.Equal("Key1", attributes[0].Key);
            Assert.Equal("Value1", attributes[0].Value);
            Assert.Equal("Key2", attributes[1].Key);
            Assert.Equal("Value2", attributes[1].Value);
        }

        [Fact]
        public void GetKeyValueAttributes_ShouldReturnEmptyList_WhenEnumValueHasNoAttributes()
        {
            // Arrange
            var enumValue = TestEnum.ValueWithoutAttributes;

            // Act
            var attributes = enumValue.GetKeyValueAttributes();

            // Assert
            Assert.NotNull(attributes);
            Assert.Empty(attributes);
        }

        [Fact]
        public void GetKeyValueAttributes_ShouldReturnEmptyList_WhenEnumValueIsNull()
        {
            // Arrange
            TestEnum? enumValue = null;

            // Act
            var attributes = enumValue.GetKeyValueAttributes();

            // Assert
            Assert.NotNull(attributes);
            Assert.Empty(attributes);
        }

        [Fact]
        public void GetKeyValueAttributes_ShouldReturnEmptyList_WhenInputIsNull()
        {
            Assert.NotNull(OltKeyValueAttributeExtensions.GetKeyValueAttributes(null));
            Assert.Empty(OltKeyValueAttributeExtensions.GetKeyValueAttributes(null));
        }
    }
}
