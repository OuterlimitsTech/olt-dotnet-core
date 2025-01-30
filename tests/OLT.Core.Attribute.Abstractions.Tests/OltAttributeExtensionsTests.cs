using System.ComponentModel;
using System.Runtime.Serialization;

namespace OLT.Core.Attribute.Abstractions.Tests;

public class OltAttributeExtensionsTests
{
    private enum TestEnum
    {
        [Description("First Value")]
        [EnumMember(Value = "first-value")]
        First,
        [Description("Second Value")]
        Second
    }

    private class TestClass
    {
        [Description("Test Property")]
        public string? TestProperty { get; set; }
    }

    [Fact]
    public void GetAttributeInstance_Enum_ReturnsCorrectAttribute()
    {
        var attribute = TestEnum.First.GetAttributeInstance<DescriptionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("First Value", attribute.Description);
    }

    [Fact]
    public void GetAttributeInstance_Property_ReturnsCorrectAttribute()
    {
        var property = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty));
        var attribute = property.GetAttributeInstance<DescriptionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Test Property", attribute.Description);
    }

    [Fact]
    public void GetDescription_ReturnsCorrectDescription()
    {
        var description = TestEnum.First.GetDescription();
        Assert.Equal("First Value", description);
    }

    [Fact]
    public void FromDescription_ReturnsCorrectEnum()
    {
        var enumValue = "First Value".FromDescription<TestEnum>();
        Assert.Equal(TestEnum.First, enumValue);
    }

    [Fact]
    public void FromEnumMember_ReturnsCorrectEnum()
    {
        var enumValue = "first-value".FromEnumMember<TestEnum>();
        Assert.Equal(TestEnum.First, enumValue);
    }

    [Fact]
    public void FromDescription_InvalidDescription_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => OltAttributeExtensions.FromDescription<TestEnum>(Faker.Name.First()));
        Assert.Throws<ArgumentNullException>(() => OltAttributeExtensions.FromDescription<TestEnum>(null));
    }

    [Fact]
    public void FromEnumMember_InvalidEnumMember_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Faker.Name.First().FromEnumMember<TestEnum>());
        Assert.Throws<ArgumentNullException>(() => OltAttributeExtensions.FromEnumMember<TestEnum>(null));
    }

}
