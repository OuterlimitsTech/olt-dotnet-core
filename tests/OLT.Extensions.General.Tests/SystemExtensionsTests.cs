using System;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class SystemExtensionsTests
    {

        [Fact]
        public void ColorToHex()
        {
            Assert.Equal("#FF0000", System.Drawing.Color.Red.ToHex());
        }

        [Fact]
        public void ColorToRGB()
        {
            Assert.Equal("RGB(240,255,255)", System.Drawing.Color.Azure.ToRGB());
        }
      
    }
}