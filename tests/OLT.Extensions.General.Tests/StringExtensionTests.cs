﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class StringExtensionTests
    {


        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", " ", " ")]
        [InlineData(null, null, null)]
        [InlineData(null, "", "")]
        [InlineData(null, "Hello", "Hello")]
        [InlineData("Test", "Hello", "Test")]
        public void GetValueOrDefault(string value, string defaultValue, string expected)
        {
            Assert.Equal(expected, value.GetValueOrDefault(defaultValue));
        }

        [Fact]
        public void RemoveDoubleSpaces()
        {
            var value = $"  {UnitTestConstants.StringValues.HelloWorld}  {UnitTestConstants.StringValues.ThisIsATest}";
            var eval = $"{UnitTestConstants.StringValues.HelloWorld} {UnitTestConstants.StringValues.ThisIsATest}";
            Assert.Equal(OltStringExtensions.RemoveDoubleSpaces(value), eval);
            Assert.Null(OltStringExtensions.RemoveDoubleSpaces(null));
        }

        [Fact]
        public void CleanForSearch()
        {
            var value = $"   -> ? {UnitTestConstants.StringValues.HelloWorld},   & {UnitTestConstants.StringValues.ThisIsATest}";
            var eval = $"{UnitTestConstants.StringValues.HelloWorld} {UnitTestConstants.StringValues.ThisIsATest}";
            Assert.Equal(OltStringExtensions.CleanForSearch(value), eval);
            Assert.Null(OltStringExtensions.CleanForSearch(null));
        }


        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData("!&*$", "")]
        [InlineData("Hello &!There", "Hello There")]
        public void RemoveSpecialCharacters(string value, string expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.RemoveSpecialCharacters(value));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", "", " ")]
        [InlineData(null, null, null)]
        [InlineData("!&*$", "", "")]
        [InlineData("!&*$", "-", "----")]
        [InlineData("Hello &!There", "", "Hello There")]
        [InlineData("Hello &!There", "_", "Hello __There")]
        public void RemoveSpecialCharactersReplaceWith(string value, string replaceWith, string expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.RemoveSpecialCharacters(value, replaceWith));
        }

        [Fact]
        public void ToWords()
        {
            Assert.Collection(UnitTestConstants.StringValues.HelloWorld.ToWords(), 
                item => Assert.Equal(UnitTestConstants.StringValues.Hello, item), 
                item => Assert.Equal(UnitTestConstants.StringValues.World, item));


            Assert.Null(OltStringExtensions.ToWords(null));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("John's", "John''s")]
        [InlineData("Johns", "Johns")]
        [InlineData("\"John's\"", "\"John''s\"")]
        public void DuplicateTicksForSql(string value, string expectedResult)
        {
            Assert.Equal(expectedResult, value.DuplicateTicksForSql());
        }

        [Theory]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, 5, UnitTestConstants.StringValues.World)]
        [InlineData("", 10, "")]
        [InlineData(null, 10, null)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, 20, UnitTestConstants.StringValues.HelloWorld)]
        public void Right(string value, int length, string expectedResult)
        {
            Assert.Equal(expectedResult, value.Right(length));
            Assert.Equal(expectedResult, value.Tail(length));
        }

        [Theory]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, 5, UnitTestConstants.StringValues.Hello)]
        [InlineData("", 10, "")]
        [InlineData(null, 10, null)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, 20, UnitTestConstants.StringValues.HelloWorld)]
        public void Left(string value, int length, string expectedResult)
        {
            Assert.Equal(expectedResult, value.Left(length));
            Assert.Equal(expectedResult, value.Head(length));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData(UnitTestConstants.GuidValues.String, true)]
        [InlineData(UnitTestConstants.GuidValues.String2, true)]
        [InlineData(UnitTestConstants.GuidValues.String3, true)]
        [InlineData(UnitTestConstants.StringValues.FooBar, false)]
        [InlineData(UnitTestConstants.StringValues.Hex, false)]
        [InlineData(UnitTestConstants.IntValues.String, false)]
        public void IsGuid(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, value.IsGuid());
        }


        [Theory]
        [InlineData(UnitTestConstants.GuidValues.String, UnitTestConstants.GuidValues.String)]
        [InlineData(UnitTestConstants.GuidValues.String, UnitTestConstants.GuidValues.String, UnitTestConstants.GuidValues.String2)]
        [InlineData(UnitTestConstants.StringValues.FooBar, null)]
        [InlineData(UnitTestConstants.StringValues.FooBar, UnitTestConstants.GuidValues.String2, UnitTestConstants.GuidValues.String2)]
        [InlineData(UnitTestConstants.StringValues.Hex, null)]
        [InlineData(UnitTestConstants.StringValues.Hex, UnitTestConstants.GuidValues.String, UnitTestConstants.GuidValues.String)]
        [InlineData(UnitTestConstants.IntValues.String, null)]
        public void ToGuid(string value, string expectedResult, string defaultValue = null)
        {
            var eval = defaultValue != null ? value.ToGuid(new Guid(defaultValue)).ToString() : value.ToGuid()?.ToString();
            Assert.Equal(expectedResult?.ToLower(), eval);
        }


        [Theory]
        [InlineData(UnitTestConstants.IntValues.String, true)]
        [InlineData(UnitTestConstants.DecimalValues.String, true)]
        [InlineData(UnitTestConstants.StringValues.FooBar, false)]
        public void IsNumeric(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.IsNumeric(value));
        }


        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(UnitTestConstants.StringValues.PhoneValues.Formatted, UnitTestConstants.StringValues.PhoneValues.Clean)]
        public void StripNonNumeric(string value, string expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.StripNonNumeric(value));
        }


        [Theory]
        [InlineData("", "", true)]
        [InlineData("", "", false)]
        [InlineData(null, null, true)]
        [InlineData(null, null, false)]
        [InlineData(UnitTestConstants.DecimalValues.String, UnitTestConstants.DecimalValues.String, true, UnitTestConstants.StringValues.HelloWorld)]
        [InlineData(UnitTestConstants.DecimalValues.String, "31415", false, UnitTestConstants.StringValues.HelloWorld)]
        public void StripNonNumericDecimal(string value, string expectedResult, bool allowDecimal, string value2 = null)
        {
            if (value == null)
            {
                Assert.Equal(expectedResult, OltStringExtensions.StripNonNumeric(value, allowDecimal));
                return;
            }
            Assert.Equal(expectedResult, OltStringExtensions.StripNonNumeric($"{value}{value2}", allowDecimal));
        }

        [Theory]
        [InlineData("", "", 10, "")]
        [InlineData(" ", " ", 10, "")]
        [InlineData(null, null, 20, null)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, UnitTestConstants.StringValues.Test, 7, "hello-world-test")]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, UnitTestConstants.StringValues.Test, 100, "hello-world-test")]
        [InlineData("$%hello", UnitTestConstants.StringValues.Test, 100, "hello-test")]
        [InlineData("$%hello-another", UnitTestConstants.StringValues.Test, 100, "hello-another-test")]
        public void Slugify(string value, string value2, int maxLength, string expected)
        {
            var testValue = value == null ? value : $"{value}   ${value2}";
            Assert.Equal(expected.Left(maxLength), OltStringExtensions.Slugify(testValue, maxLength));
        }

        [Theory]
        [InlineData("", "", ", ", "")]
        [InlineData(" ", " ", ", ", " ,  ")]
        [InlineData(null, null, ", ", "")]
        [InlineData(null, "Hello", ", ", "Hello")]
        [InlineData("Hello", "There", " ", "Hello There")]
        [InlineData("Hello", "There", ",", "Hello,There")]
        public void Append(string value, string value2, string separator, string expected)
        {
            Assert.Equal(expected, OltStringExtensions.Append(value, value2, separator));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData("Hello There", "Hello there")]
        [InlineData("hello there", "Hello there")]
        [InlineData("hello There", "Hello there")]
        [InlineData("HELLO THERE CHARLIE Brown", "Hello there charlie brown")]
        public void ToProperCase(string value, string expected)
        {
            Assert.Equal(expected, OltStringExtensions.ToProperCase(value));
        }



        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData("9/1/2021", true)]
        [InlineData("2021-09-01", true)]
        public void IsDate(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.IsDate(value));
        }


        public static IEnumerable<object[]> ToDateMemberData =>
            new List<object[]>
            {
                new object[] { "", null, null },
                new object[] { null, null, null },
                new object[] { "", DateTime.Today, DateTime.Today },
                new object[] { null, DateTime.Today, DateTime.Today },
                new object[] { UnitTestConstants.DateTimeValues.String, UnitTestConstants.DateTimeValues.Value, null },
            };


        [Theory]
        [MemberData(nameof(ToDateMemberData))]
        public void ToDate(string value, DateTime? expectedResult, DateTime? defaultValue = null)
        {
            Assert.Equal(expectedResult, defaultValue.HasValue ? OltStringExtensions.ToDate(value, defaultValue.Value) : OltStringExtensions.ToDate(value));
        }



        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData(" ", null)]
        [InlineData("FooBar", null)]
        [InlineData("930248", 930248)]
        [InlineData("54", 54)]
        [InlineData("45.234", null)]
        [InlineData("-1", -1)]
        public void ToInt(string value, int? expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.ToInt(value));
        }

        [Theory]
        [InlineData("", int.MaxValue, int.MaxValue)]
        [InlineData("", -100, -100)]
        [InlineData(null, -150,-150)]
        [InlineData(" ", 0, 0)]
        [InlineData("FooBar", 0, 0)]
        [InlineData(null, 930248, 930248)]
        [InlineData("45.234", 950, 950)]
        [InlineData("-1", -1, 10000)]
        public void ToIntWithDefault(string value, int? expectedResult, int defaultValue)
        {
            Assert.Equal(expectedResult, OltStringExtensions.ToInt(value, defaultValue));
        }

        [Fact]
        public void ToIntOverflow()
        {
            long num = int.MaxValue;
            var value = (num + 1).ToString();
            Assert.Null(OltStringExtensions.ToInt(value));
            Assert.Equal(1234, OltStringExtensions.ToInt(value, 1234));
        }


        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData(" ", null)]
        [InlineData("FooBar", null)]
        [InlineData("9223372036854775800", 9223372036854775800)]
        [InlineData("45.234", null)]
        [InlineData("-9223372036854775800", -9223372036854775800)]
        [InlineData("-1", -1L)]
        public void ToLong(string value, long? expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.ToLong(value));
        }

        [Theory]
        [InlineData("-1", -1)]
        public void ToLongInt(string value, int? expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.ToLong(value));
        }

        [Theory]
        [InlineData("", long.MaxValue, long.MaxValue)]
        [InlineData("", 9223372036854775800, 9223372036854775800)]
        [InlineData(null, long.MaxValue, long.MaxValue)]
        [InlineData(" ", 100L, 100L)]
        [InlineData("FooBar", -9223372036854775800, -9223372036854775800)]        
        [InlineData("45.234", 9223372036854775800, 9223372036854775800)]
        [InlineData("-9223372036854775800", -9223372036854775800, 0)]
        [InlineData("-1", -1L, 0L)]
        public void ToLongWithDefault(string value, long? expectedResult, long defaultValue)
        {
            Assert.Equal(expectedResult, OltStringExtensions.ToLong(value, defaultValue));
        }

        [Fact]
        public void ToLongOverflow()
        {
            long num = long.MaxValue;
            var value = $"{num}5";
            Assert.Null(OltStringExtensions.ToLong(value));
            Assert.Equal(long.MinValue, OltStringExtensions.ToLong(value, long.MinValue));
        }

        public static IEnumerable<object[]> ToDecimalMemberData =>
            new List<object[]>
            {
                new object[] { null, decimal.MaxValue, decimal.MaxValue },
                new object[] { "", null },
                new object[] { null, null },
                new object[] { "", 0.0m, 0.0m },
                new object[] { "FooBar", 2.133m, 2.133m },
                new object[] { "45.234", 45.234m, 0.0m },
                new object[] { "35", 35m },
                new object[] { null, 45.234m, 45.234m },
                new object[] { "-1", -1m },
            };


        [Theory]
        [MemberData(nameof(ToDecimalMemberData))]
        public void ToDecimal(string value, decimal? expectedResult, decimal? defaultValue = null)
        {
            Assert.Equal(expectedResult, defaultValue.HasValue ? OltStringExtensions.ToDecimal(value, defaultValue.Value) : OltStringExtensions.ToDecimal(value));
        }

        [Theory]
        [InlineData("", double.MaxValue, double.MaxValue)]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData(" ", 0d, 0d)]
        [InlineData(null, 1.01, 1.01)]
        [InlineData("FooBar", 0d, 0d)]
        [InlineData("45.234", 45.234, 0.0)]
        [InlineData("-1", -1d)]
        public void ToDouble(string value, double? expectedResult, double? defaultValue = null)
        {
            Assert.Equal(expectedResult, defaultValue.HasValue ? OltStringExtensions.ToDouble(value, defaultValue.Value) : OltStringExtensions.ToDouble(value));
        }


        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData(UnitTestConstants.BoolValues.TrueValues.String, true)]
        [InlineData(UnitTestConstants.BoolValues.TrueValues.Int, true)]
        [InlineData(UnitTestConstants.StringValues.AlphaNumeric, false)]
        [InlineData(UnitTestConstants.BoolValues.FalseValues.String, true)]
        [InlineData(UnitTestConstants.BoolValues.FalseValues.Int, true)]
        public void IsBool(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, value.IsBool());
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", true, true)]
        [InlineData("", false, false)]
        [InlineData(" ", false, false)]
        [InlineData(null, null)]
        [InlineData(null, true, true)]
        [InlineData(null, false, false)]
        [InlineData(UnitTestConstants.BoolValues.TrueValues.String, true)]
        [InlineData(UnitTestConstants.BoolValues.TrueValues.String, true, false)]
        [InlineData(UnitTestConstants.BoolValues.TrueValues.Int, true)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, null)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, true, true)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, false, false)]
        [InlineData(UnitTestConstants.BoolValues.FalseValues.String, false)]
        [InlineData(UnitTestConstants.BoolValues.FalseValues.String, false, true)]
        [InlineData(UnitTestConstants.BoolValues.FalseValues.Int, false)]
        public void ToBool(string value, bool? expectedResult, bool? defaultValue = null)
        {
            Assert.Equal(expectedResult, defaultValue.HasValue ? OltStringExtensions.ToBool(value, defaultValue.Value) : OltStringExtensions.ToBool(value));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData("d7980689965b", true)]
        [InlineData("123987", true)]
        [InlineData(UnitTestConstants.StringValues.Hex, true)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, false)]
        public void IsHex(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, OltStringExtensions.IsHex(value));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData(UnitTestConstants.StringValues.Hex, true)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, false)]
        public void FromHexToByte(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, value.FromHexToByte().Length > 0);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData(UnitTestConstants.StringValues.Hello, UnitTestConstants.StringValues.HelloReverse)]
        [InlineData("1234ABC", "CBA4321")]
        [InlineData(" 1234ABC ", " CBA4321 ")]
        [InlineData(" 1234ABC Testing", "gnitseT CBA4321 ")]
        public void Reverse(string value, string expectedResult)
        {
            Assert.Equal(expectedResult, value.Reverse());
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData(UnitTestConstants.StringValues.HelloReverse, false)]
        [InlineData(UnitTestConstants.StringValues.ThisIsATest, true)]
        [InlineData(UnitTestConstants.StringValues.HelloWorld, true)]
        public void StartsWithAny(string value, bool expectedResult)
        {
            Assert.Equal(value.StartsWithAny(UnitTestConstants.StringValues.Test, UnitTestConstants.StringValues.Hello, UnitTestConstants.StringValues.This), expectedResult);
        }


        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData(UnitTestConstants.StringValues.HelloReverse, false)]
        [InlineData(UnitTestConstants.StringValues.ThisIsATest, true)]
        public void EqualsAny(string value, bool expectedResult)
        {
            Assert.Equal(expectedResult, value.EqualsAny(UnitTestConstants.StringValues.ThisIsATest, UnitTestConstants.StringValues.Hex));
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(UnitTestConstants.StringValues.Hello, false)]
        public void DBNullIfEmpty(string value, bool expectedResult)
        {
            Assert.Equal(value.DBNullIfEmpty().Equals(DBNull.Value), expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Hello There")]
        [InlineData("HelloThere")]
        public void Base64EncodeDecode(string value)
        {
            var encoded = value.Base64Encode();
            Assert.Equal(value, encoded.Base64Decode());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        [InlineData("Hello There", "Hello There")]
        [InlineData("HelloThere", "Hello there")]
        [InlineData("HelloThereSally", "Hello there sally")]
        public void ToSentenceCase(string value, string expectedResult)
        {
            Assert.Equal(expectedResult, value.ToSentenceCase());
        }

        [Theory]
        [InlineData("κόσμε", 11)]
        [InlineData("a", 1)] //1 byte
        [InlineData("Ć", 2)] //2 bytes
        [InlineData("ꦀ", 3)] //3 bytes - Javanese
        [InlineData("𒀃", 4)] //4 bytes - Sumerian cuneiform
        [InlineData("a𒀃", 5)] //5 bytes
        [InlineData(null, 0)]
        public void ToUTFByteTests(string value, int expectedLength)
        {
            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(() => OltStringExtensions.ToUTF8Bytes(value));
            }
            else
            {
                var bytes = OltStringExtensions.ToUTF8Bytes(value);
                Assert.NotNull(bytes);
                Assert.Equal(System.Text.Encoding.UTF8.GetString(bytes), value);
                Assert.Equal(expectedLength, bytes.Length);
            }
        }


        [Theory]
        [InlineData('')]
        [InlineData('')]
        [InlineData('')]
        [InlineData('')]
        public void ToUTFByteCharTests(char value)
        {
            var bytes = OltStringExtensions.ToUTF8Bytes(value);
            Assert.NotNull(bytes);
            Assert.Equal(System.Text.Encoding.UTF8.GetString(bytes), value.ToString());
        }

        [Theory]
        [InlineData(" ", 1)]
        [InlineData("", 0)]
        [InlineData("123ABC", 6)]
        [InlineData(null, 0)]
        public void ToASCIIByteTests(string value, int expectedLength)
        {
            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(() => OltStringExtensions.ToASCIIBytes(value));
            }
            else
            {
                var bytes = OltStringExtensions.ToASCIIBytes(value);
                Assert.NotNull(bytes);
                Assert.Equal(System.Text.Encoding.ASCII.GetString(bytes), value);
                Assert.Equal(expectedLength, bytes.Length);
            }
        }

        [Theory]
        [InlineData('A')]
        [InlineData('Z')]
        [InlineData('9')]
        [InlineData('l')]
        public void ToASCIIByteCharTests(char value)
        {
            var bytes = OltStringExtensions.ToASCIIBytes(value);
            Assert.NotNull(bytes);
            Assert.Equal(System.Text.Encoding.UTF8.GetString(bytes), value.ToString());
        }        

    }
}