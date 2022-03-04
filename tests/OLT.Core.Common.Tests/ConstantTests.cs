using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class ConstantTests
    {

        [Fact]
        public void OltCharactersTests()
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

        [Fact]
        public void OltEnvironmentsTests()
        {
            Assert.Equal("Development", OltEnvironments.Development);
            Assert.Equal("Test", OltEnvironments.Test);
            Assert.Equal("Staging", OltEnvironments.Staging);
            Assert.Equal("Production", OltEnvironments.Production);
        }
    }
}
