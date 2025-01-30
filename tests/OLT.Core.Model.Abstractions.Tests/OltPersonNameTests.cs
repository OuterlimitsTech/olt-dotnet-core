namespace OLT.Core.Model.Abstractions.Tests;

public class OltPersonNameTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange & Act
        var person = new OltPersonName("John", "Doe");

        // Assert
        Assert.Equal("John", person.First);
        Assert.Equal("Doe", person.Last);
        Assert.Null(person.Middle);
        Assert.Null(person.Suffix);
    }

    [Fact]
    public void Constructor_WithMiddleAndSuffix_ShouldInitializeProperties()
    {
        // Arrange & Act
        var person = new OltPersonName("John", "Michael", "Doe", "Jr");

        // Assert
        Assert.Equal("John", person.First);
        Assert.Equal("Michael", person.Middle);
        Assert.Equal("Doe", person.Last);
        Assert.Equal("Jr", person.Suffix);
    }

    [Fact]
    public void FullName_ShouldReturnCorrectFormat()
    {
        // Arrange
        var person = new OltPersonName("John", "Michael", "Doe", "Jr");

        // Act
        var fullName = person.FullName;

        // Assert
        Assert.Equal("John Michael Doe Jr", fullName);
    }

    [Fact]
    public void FullName_ShouldHandleNullValues()
    {
        // Arrange
        var person = new OltPersonName("John", null, "Doe", null);

        // Act
        var fullName = person.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void FullName_ShouldHandleEmptyValues()
    {
        // Arrange
        var person = new OltPersonName("John", "", "Doe", "");

        // Act
        var fullName = person.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Theory]
    [InlineData("Test Jones", "Test", null, "Jones", null)]
    [InlineData("Test M Jones", "Test", "M", "Jones", null)]
    [InlineData("Test M Jones Jr", "Test", "M", "Jones", "Jr")]
    [InlineData("M Jones Jr", null, "M", "Jones", "Jr")]
    [InlineData("M Jr", null, "M", null, "Jr")]
    [InlineData("", null, null, null, null)]
    public void Constructor_WithVariousInputs_ShouldInitializePropertiesCorrectly(string expected, string? first, string? middle, string? last, string? suffix)
    {
        var model = new OltPersonName(first, middle, last, suffix);

        Assert.NotNull(model as IOltPersonName);

        Assert.Equal(first, model.First);
        Assert.Equal(middle, model.Middle);
        Assert.Equal(last, model.Last);
        Assert.Equal(suffix, model.Suffix);
        Assert.Equal(expected, model.FullName);


        model = new OltPersonName(first, last);
        Assert.Equal(first, model.First);
        Assert.Null(model.Middle);
        Assert.Equal(last, model.Last);
        Assert.Null(model.Suffix);
        Assert.Equal(System.Text.RegularExpressions.Regex.Replace(($"{first} {last}").Trim(), @"\s+", " "), model.FullName);
    }

}
