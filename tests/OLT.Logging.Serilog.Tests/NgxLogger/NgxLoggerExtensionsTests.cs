using Serilog.Events;
using Xunit;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{
    public class NgxLoggerExtensionsTests
    {
        [Theory]
        [InlineData(OltNgxLoggerLevel.Trace, LogEventLevel.Verbose)]
        [InlineData(OltNgxLoggerLevel.Debug, LogEventLevel.Debug)]
        [InlineData(OltNgxLoggerLevel.Information, LogEventLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Log, LogEventLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Warning, LogEventLevel.Warning)]
        [InlineData(OltNgxLoggerLevel.Error, LogEventLevel.Error)]
        [InlineData(OltNgxLoggerLevel.Fatal, LogEventLevel.Fatal)]
        [InlineData(OltNgxLoggerLevel.Off, LogEventLevel.Information)]
        public void ToLogLevelTest(OltNgxLoggerLevel value, LogEventLevel expected)
        {
            Assert.Equal(expected, OltNgxLoggerExtensions.ToLogLevel(value));
        }

        [Fact]
        public void TestDefault()
        {
            var invalidValue = (OltNgxLoggerLevel)1000;
            Assert.Equal(LogEventLevel.Information, OltNgxLoggerExtensions.ToLogLevel(invalidValue));
        }
    }
}
