using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{
    public class NgxLoggerExtensionsTests
    {
        [Theory]
        [InlineData(OltNgxLoggerLevel.Trace, LogLevel.Trace)]
        [InlineData(OltNgxLoggerLevel.Debug, LogLevel.Debug)]
        [InlineData(OltNgxLoggerLevel.Information, LogLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Log, LogLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Warning, LogLevel.Warning)]
        [InlineData(OltNgxLoggerLevel.Error, LogLevel.Error)]
        [InlineData(OltNgxLoggerLevel.Fatal, LogLevel.Critical)]
        [InlineData(OltNgxLoggerLevel.Off, LogLevel.None)]
        public void ToLogLevelTest(OltNgxLoggerLevel value, LogLevel expected)
        {
            Assert.Equal(expected, OltNgxLoggerExtensions.ToLogLevel(value));
        }

        [Fact]
        public void TestDefault()
        {
            var invalidValue = (OltNgxLoggerLevel)1000;
            Assert.Equal(LogLevel.Information, OltNgxLoggerExtensions.ToLogLevel(invalidValue));
        }
    }
}
