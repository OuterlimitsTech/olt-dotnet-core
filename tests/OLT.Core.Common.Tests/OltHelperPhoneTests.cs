////using OLT.Core;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Xunit;

////namespace OLT.Extensions.General.Tests
////{
////    public class OltHelperPhoneTests
////    {
////        [Theory]
////        [InlineData("(317) 867-5309", "3178675309", null)]
////        [InlineData("(317) 867-5309 x987", "3178675309", "987")]
////        [InlineData(null, null, null)]
////        [InlineData(null, null, "123")]
////        [InlineData("", "", "")]
////        [InlineData("(317) 867-5309", "317-867-5309", null)]
////        [InlineData("(317) 867-5309", "(317) 867-5309", null)]
////        [InlineData("(317) 867-5309 x731", "(317) 867-5309", "x731")]
////        public void FormatPhone(string expected, string phoneNumber, string ext)
////        {
////            Assert.Equal(expected, OltHelpers.Phone.Format(phoneNumber, ext));
////            Assert.Equal(expected, phoneNumber.ToFormattedPhone(ext));
////        }


////        [Theory]
////        [InlineData("(317) 867-5309", "3178675309")]
////        [InlineData(null, null)]
////        [InlineData("", "")]
////        [InlineData("(317) 867-5309", "(317) 867-5309")]
////        [InlineData("(317) 867-5309", "317-867-5309")]
////        public void ToFormattedPhone(string expected, string phoneNumber)
////        {
////            Assert.Equal(expected, phoneNumber.ToFormattedPhone());
////        }
////    }


////    public class OltHelperAddressTests
////    {
////        [Theory]
////        [InlineData("01234-9876", "012349876")]
////        [InlineData("01234-9876", "01234-9876")]
////        [InlineData("01234-9876", "01234 -9876")]
////        [InlineData(null, null)]
////        [InlineData("123", "123")]
////        [InlineData(null, "")]
////        public void FormatPhone(string expected, string postalCode)
////        {
////            Assert.Equal(expected, OltHelpers.Address.FormatPostalCode(postalCode));
////        }



////    }
////}
