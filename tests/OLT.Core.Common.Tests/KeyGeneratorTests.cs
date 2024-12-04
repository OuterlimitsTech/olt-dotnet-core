//using OLT.Constants;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace OLT.Core.Common.Tests;

//public class KeyGeneratorTests
//{
//    [Theory]
//    [InlineData(16, true, true, true, true, 16)]
//    [InlineData(12, false, true, true, true, 12)]
//    [InlineData(13, true, false, true, true, 13)]
//    [InlineData(18, true, true, false, true, 18)]
//    [InlineData(24, true, true, true, false, 24)]
//    [InlineData(8, false, false, false, false, 0)]
//    public void GeneratePassword(int length, bool useNumbers, bool useLowerCaseLetters, bool useUpperCaseLetters, bool useSymbols, int expectedLength)
//    {
//        if (expectedLength > 0)
//        {
//            Assert.True(OltKeyGenerator.GeneratePassword(length, useNumbers, useLowerCaseLetters, useUpperCaseLetters, useSymbols).Length == expectedLength);
//        }
//        else
//        {
//            Assert.Throws<ArgumentOutOfRangeException>(() => OltKeyGenerator.GeneratePassword(length, useNumbers, useLowerCaseLetters, useUpperCaseLetters, useSymbols));
//        }
//    }

//    [Theory]
//    [InlineData(0)]
//    [InlineData(25)]
//    [InlineData(10)]
//    public void GetUniqueKey(int size)
//    {
//        var password = OltKeyGenerator.GetUniqueKey(size);
//        Assert.True(password.Length == size);
//    }

//    [Theory]
//    [InlineData(1000000, 32, 2.0)]
//    [InlineData(1000000, 64, 2.0)]
//    public void GetUniqueKeyBiasedIteration(int repetitions, int keySize, double threshold)
//    {
//        var result = KeyGeneratorPerformTest(repetitions, keySize, OltKeyGenerator.GetUniqueKeyOriginal_BIASED);
//        Assert.DoesNotContain(result, p => p.Value > threshold);
//    }

//    [Theory]
//    [InlineData(1000000, 32, 1.65)]
//    [InlineData(1000000, 64, 1.65)]
//    public void GetUniqueKeyIteration(int repetitions, int keySize, double threshold)
//    {
//        var result = KeyGeneratorPerformTest(repetitions, keySize, OltKeyGenerator.GetUniqueKey);
//        Assert.DoesNotContain(result, p => p.Value > threshold);
//    }

//    private Dictionary<char, double> KeyGeneratorPerformTest(int repetitions, int keySize, Func<int, string> generator)
//    {
//        var result = new Dictionary<char, double>();
//        var chars = (new char[0])
//            .Concat(OltCharacters.UpperCase)
//            .Concat(OltCharacters.LowerCase)
//            .Concat(OltCharacters.Numerals)
//            .ToArray();

//        Dictionary<char, int> counts = new Dictionary<char, int>();
//        foreach (var ch in chars) counts.Add(ch, 0);

//        for (int i = 0; i < repetitions; i++)
//        {
//            var key = generator(keySize);
//            foreach (var ch in key) counts[ch]++;
//        }

//        int totalChars = counts.Values.Sum();
//        foreach (var ch in chars)
//        {
//            var val = 100.0 * counts[ch] / totalChars;
//            result.Add(ch, val);
//        }

//        return result;
//    }

//}