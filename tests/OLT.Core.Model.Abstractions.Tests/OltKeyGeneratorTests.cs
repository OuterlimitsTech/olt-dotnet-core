using OLT.Constants;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltKeyGeneratorTests
{
    [Fact]
    public void GeneratePassword_ShouldReturnPasswordOfSpecifiedLength()
    {
        // Arrange
        int length = 12;

        // Act
        string password = OltKeyGenerator.GeneratePassword(length);

        // Assert
        Assert.Equal(length, password.Length);
    }

    [Fact]
    public void GeneratePassword_ShouldIncludeNumbers_WhenUseNumbersIsTrue()
    {
        // Arrange
        bool useNumbers = true;

        // Act
        string password = OltKeyGenerator.GeneratePassword(useNumbers: useNumbers);

        // Assert
        Assert.Contains(password, char.IsDigit);
    }

    [Fact]
    public void GeneratePassword_ShouldIncludeLowerCaseLetters_WhenUseLowerCaseLettersIsTrue()
    {
        // Arrange
        bool useLowerCaseLetters = true;

        // Act
        string password = OltKeyGenerator.GeneratePassword(useLowerCaseLetters: useLowerCaseLetters);

        // Assert
        Assert.Contains(password, char.IsLower);
    }

    [Fact]
    public void GeneratePassword_ShouldIncludeUpperCaseLetters_WhenUseUpperCaseLettersIsTrue()
    {
        // Arrange
        bool useUpperCaseLetters = true;

        // Act
        string password = OltKeyGenerator.GeneratePassword(useUpperCaseLetters: useUpperCaseLetters);

        // Assert
        Assert.Contains(password, char.IsUpper);
    }

    [Fact]
    public void GeneratePassword_ShouldIncludeSymbols_WhenUseSymbolsIsTrue()
    {
        // Arrange
        bool useSymbols = true;

        // Act
        string password = OltKeyGenerator.GeneratePassword(useSymbols: useSymbols);

        // Assert
        Assert.Contains(password, ch => !char.IsLetterOrDigit(ch));
    }


    [Fact]
    public void GetUniqueKeyOriginal_BIASED_ShouldReturnKeyOfSpecifiedSize()
    {
        // Arrange
        int size = 16;

        // Act
        string key = OltKeyGenerator.GetUniqueKeyOriginal_BIASED(size);

        // Assert
        Assert.Equal(size, key.Length);
    }

    [Theory]
    [InlineData(16, true, true, true, true, 16)]
    [InlineData(12, false, true, true, true, 12)]
    [InlineData(13, true, false, true, true, 13)]
    [InlineData(18, true, true, false, true, 18)]
    [InlineData(24, true, true, true, false, 24)]
    [InlineData(8, false, false, false, false, 0)]
    public void GeneratePassword_ShouldReturnExpectedLength(int length, bool useNumbers, bool useLowerCaseLetters, bool useUpperCaseLetters, bool useSymbols, int expectedLength)
    {
        if (expectedLength > 0)
        {
            Assert.True(OltKeyGenerator.GeneratePassword(length, useNumbers, useLowerCaseLetters, useUpperCaseLetters, useSymbols).Length == expectedLength);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OltKeyGenerator.GeneratePassword(length, useNumbers, useLowerCaseLetters, useUpperCaseLetters, useSymbols));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(10)]
    public void GetUniqueKey_ShouldReturnKeyOfSpecifiedSize(int size)
    {
        var password = OltKeyGenerator.GetUniqueKey(size);
        Assert.True(password.Length == size);
    }

    [Theory]
    [InlineData(1000000, 32, 2.0)]
    [InlineData(1000000, 64, 2.0)]
    public void GetUniqueKeyBiasedIteration_ShouldNotExceedThreshold(int repetitions, int keySize, double threshold)
    {
        var result = KeyGeneratorPerformTest(repetitions, keySize, OltKeyGenerator.GetUniqueKeyOriginal_BIASED);
        Assert.DoesNotContain(result, p => p.Value > threshold);
    }

    [Theory]
    [InlineData(1000000, 32, 1.65)]
    [InlineData(1000000, 64, 1.65)]
    public void GetUniqueKeyIteration_ShouldNotExceedThreshold(int repetitions, int keySize, double threshold)
    {
        var result = KeyGeneratorPerformTest(repetitions, keySize, OltKeyGenerator.GetUniqueKey);
        Assert.DoesNotContain(result, p => p.Value > threshold);
    }

    private Dictionary<char, double> KeyGeneratorPerformTest(int repetitions, int keySize, Func<int, string> generator)
    {
        var result = new Dictionary<char, double>();
        var chars = (new char[0])
            .Concat(OltCharacters.UpperCase)
            .Concat(OltCharacters.LowerCase)
            .Concat(OltCharacters.Numerals)
            .ToArray();

        Dictionary<char, int> counts = new Dictionary<char, int>();
        foreach (var ch in chars) counts.Add(ch, 0);

        for (int i = 0; i < repetitions; i++)
        {
            var key = generator(keySize);
            foreach (var ch in key) counts[ch]++;
        }

        int totalChars = counts.Values.Sum();
        foreach (var ch in chars)
        {
            var val = 100.0 * counts[ch] / totalChars;
            result.Add(ch, val);
        }

        return result;
    }

}
