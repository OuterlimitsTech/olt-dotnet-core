using FluentAssertions;
using OLT.Core;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Xunit.Abstractions;

namespace OLT.Logging.Serilog.Tests
{

    public class SerilogTests
    {
        private readonly ITestOutputHelper _output;

        public SerilogTests(ITestOutputHelper output)
        {
            _output = output;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }

        [Fact]
        public void DefaultEmailTemplateTests()
        {
            var expected = $"{Environment.NewLine}{Environment.NewLine}{OltSerilogConstants.Templates.DefaultOutput}{Environment.NewLine}{Environment.NewLine}";
            Assert.Equal(expected, OltSerilogConstants.Templates.Email.DefaultEmail);
        }

        [Fact]
        public void WithOltEventType()
        {
            var val = 1234;

            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration()
                .Enrich.WithOltEventType()
                .WriteTo.TestOutput(_output, outputTemplate: OltSerilogConstants.Templates.DefaultOutput)
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger())
            {
                logger.Information("Test Log Value: {value1}", val);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .SelectMany(s => s.Properties.Select(t => t.Key))
                    .Where(key => key == OltSerilogConstants.Properties.EventType)
                    .Should()
                    .ContainSingle();
                TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.MessageTemplate.Text.Should().Be("Test Log Value: {value1}");
            }
        }


        [Fact]
        public void WithOltEnvironment()
        {
            var val = 1234;

            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration()
                .Enrich.WithOltEnvironment("foobar")
                .WriteTo.TestOutput(_output, outputTemplate: OltSerilogConstants.Templates.DefaultOutput)
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger())
            {
                logger.Information("Test Log Value: {value1}", val);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .SelectMany(s => s.Properties.Select(t => t.Key))
                    .Where(key => key == OltSerilogConstants.Properties.Environment)
                    .Should()
                    .ContainSingle();

                TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.MessageTemplate.Text.Should().Be("Test Log Value: {value1}");
            }
        }
    }
}