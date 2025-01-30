using OLT.Constants;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltCharactersTests
{
    [Fact]
    public void UpperCase_ShouldContainAllUpperCaseLetters()
    {
        char[] expected = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        Assert.Equal(expected, OltCharacters.UpperCase);
    }

    [Fact]
    public void LowerCase_ShouldContainAllLowerCaseLetters()
    {
        char[] expected = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        Assert.Equal(expected, OltCharacters.LowerCase);
    }

    [Fact]
    public void Numerals_ShouldContainAllNumerals()
    {
        char[] expected = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        Assert.Equal(expected, OltCharacters.Numerals);
    }

    [Fact]
    public void Symbols_ShouldContainAllSymbols()
    {
        char[] expected = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '[', '}', ']', '-', '_', '=', '+', ':', ';', '|', '/', '?', ',', '<', '.', '>' };
        Assert.Equal(expected, OltCharacters.Symbols);
    }

    [Fact]
    public void Special_ShouldContainAllSpecialCharacters()
    {
        char[] expected = { '!', '@', '#', '$', '%', '&', '*', '+' };
        Assert.Equal(expected, OltCharacters.Special);
    }

    [Fact]
    public void CharacterArrays_ShouldHaveCorrectLengthsAndUniqueElements()
    {
        Assert.Equal(26, OltCharacters.UpperCase.Length);
        Assert.Equal(26, OltCharacters.UpperCase.Distinct().Count());

        Assert.Equal(26, OltCharacters.LowerCase.Length);
        Assert.Equal(26, OltCharacters.LowerCase.Distinct().Count());

        Assert.Equal(10, OltCharacters.Numerals.Length);
        Assert.Equal(10, OltCharacters.Numerals.Distinct().Count());

        Assert.Equal(29, OltCharacters.Symbols.Length);
        Assert.Equal(29, OltCharacters.Symbols.Distinct().Count());

        Assert.Equal(8, OltCharacters.Special.Length);
        Assert.Equal(8, OltCharacters.Special.Distinct().Count());
    }

}
