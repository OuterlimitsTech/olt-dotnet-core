using FluentAssertions;
using OLT.Constants;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{

    public class NgxLoggerMessageTests
    {
        private readonly ITestOutputHelper _output;

        public NgxLoggerMessageTests(ITestOutputHelper output)
        {
            _output = output;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }


        [Theory]
        [MemberData(nameof(NgxLoggerMessageData))]
        public void MessageTest(OltNgxLoggerLevel? level, bool loadDetail, bool expectedError, HelperNgxExceptionTest data)
        {
            var msg = data.BuildMessage(level, loadDetail ? data.Detail : null);

            if (!loadDetail && level == OltNgxLoggerLevel.Fatal)
            {
                msg.Additional.Add(new List<OltNgxLoggerDetailJson>());
            }

            var exception = msg.ToException();
            var exceptionMessage = msg.Additional.FirstOrDefault()?.FirstOrDefault()?.Message ?? msg.Message;
            Assert.Equal(exceptionMessage, exception.Message);

            var formattedLogMsg = msg.FormatMessage();
            Assert.Equal(exception.Message, formattedLogMsg);

            Assert.Equal(expectedError, msg.IsError);

            var dict = TestHelper.ToDictionary(exception.Data);
            if (loadDetail)
            {
                Assert.Collection(dict,
                    item => Assert.Equal(data.Result["Name"], item.Value),
                    item => Assert.Equal(data.Result["AppId"], item.Value),
                    item => Assert.Equal(data.Result["User"], item.Value),
                    item => Assert.Equal(data.Result["Time"], item.Value),
                    item => Assert.Equal(data.Result["Url"], item.Value),
                    item => Assert.Equal(data.Result["Status"], item.Value),
                    item => Assert.Equal(data.Result["Stack"], item.Value),
                    item => Assert.Equal(data.Result["Username"], item.Value),
                    item => Assert.Equal(data.Result["Level"], item.Value),
                    item => Assert.Equal(data.Result["FileName"], item.Value),
                    item => Assert.Equal(data.Result["LineNumber"], item.Value),
                    item => Assert.Equal(data.Result["ColumnNumber"], item.Value),                    
                    item => Assert.Equal(data.Result["Timestamp"], item.Value)
                );

                return;
            }


            Assert.Collection(dict,
                item => Assert.Equal(data.Result["Username"], item.Value),
                item => Assert.Equal(data.Result["Level"], item.Value),
                item => Assert.Equal(data.Result["FileName"], item.Value),
                item => Assert.Equal(data.Result["LineNumber"], item.Value),
                item => Assert.Equal(data.Result["ColumnNumber"], item.Value),                
                item => Assert.Equal(data.Result["Timestamp"], item.Value)
            );

        }


        public static IEnumerable<object[]> NgxLoggerMessageData =>
         new List<object[]>
         {
                new object[] { OltNgxLoggerLevel.Error, true, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Fatal, true, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Information, true, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, true, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Trace, true, false, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Warning, true, false, new HelperNgxExceptionTest(null) },

                new object[] { OltNgxLoggerLevel.Error, false, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Fatal, false, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Warning, false, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, false, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, false, false, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Debug, false, false, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Log, false, false, new HelperNgxExceptionTest(null) },

                new object[] { OltNgxLoggerLevel.Error, false, true, new HelperNgxExceptionTest(DateTimeOffset.Now, true) },
                new object[] { OltNgxLoggerLevel.Error, false, true, new HelperNgxExceptionTest(null, true) },

         };


        private async Task<OltNgxLoggerMessageJson> FromJsonFile()
        {
            string fileName = "ngx-sample.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, "NgxLogger", fileName);

            OltNgxLoggerMessageJson result;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };

            using (FileStream openStream = File.OpenRead(filePath))
            {
                result = await JsonSerializer.DeserializeAsync<OltNgxLoggerMessageJson>(openStream, options);
            }

            return result;
        }

        [Fact]
        public async Task ToDictionaryTests()
        {
            var model = await FromJsonFile();

            var dict = model.ToDictionary();
            dict.Should().HaveCount(47);


            //dict.ToList().ForEach(item =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"expected.Add(\"{item.Key}\", \"{item.Value}\");");                
            //});

            var expected = new Dictionary<string, string>();
            expected.Add("ngx-message.Message", "Http failure response for https://localhost:45687/api/customers/1111/images/base64?aspectValue=80&aspectRatio=width&api-version=1.0: 404 OK");
            expected.Add("ngx-message.Level", "Fatal");
            expected.Add("ngx-message.Timestamp", "3/2/2022 10:06:42 PM +00:00");
            expected.Add("ngx-message.FileName", "vendor.js");
            expected.Add("ngx-message.LineNumber", "81349");
            expected.Add("ngx-message.ColumnNumber", "21");
            expected.Add("ngx-message.IsError", "true");
            expected.Add("ngx-detail[0].Name", "HttpErrorResponse");
            expected.Add("ngx-detail[0].AppId", "my-app");
            expected.Add("ngx-detail[0].User", "test.user@testing.gov");
            expected.Add("ngx-detail[0].Time", "1646258802617");
            expected.Add("ngx-detail[0].Id", "app-test.user@testing.gov-1646258802617");
            expected.Add("ngx-detail[0].Url", null);
            expected.Add("ngx-detail[0].Status", null);
            expected.Add("ngx-detail[0].Message", "Http failure response for https://localhost:45687/api/customers/1111/images/base64?aspectValue=80&aspectRatio=width&api-version=1.0: 500 OK");
            expected.Add("ngx-detail[0].Stack", null);
            expected.Add("ngx-detail[1].Name", "HttpErrorResponse");
            expected.Add("ngx-detail[1].AppId", "my-app");
            expected.Add("ngx-detail[1].User", "test.user@testing.gov");
            expected.Add("ngx-detail[1].Time", "1646258802617");
            expected.Add("ngx-detail[1].Id", "app-test.user@testing.gov-1646258802617");
            expected.Add("ngx-detail[1].Url", "/queues/my-queue");
            expected.Add("ngx-detail[1].Status", "404");
            expected.Add("ngx-detail[1].Message", "Http failure response for https://localhost:45687/api/customers/1111/images/base64?aspectValue=80&aspectRatio=width&api-version=1.0: 404 OK");
            expected.Add("ngx-detail[1].Stack[0].ColumnNumber", "45");
            expected.Add("ngx-detail[1].Stack[0].LineNumber", "131598");
            expected.Add("ngx-detail[1].Stack[0].FileName", "vendor.js");
            expected.Add("ngx-detail[1].Stack[0].FunctionName", "getValue");
            expected.Add("ngx-detail[1].Stack[0].Source", "foobar");
            expected.Add("ngx-detail[2].Name", "HttpErrorResponse");
            expected.Add("ngx-detail[2].AppId", "my-app");
            expected.Add("ngx-detail[2].User", "test.user@testing.gov");
            expected.Add("ngx-detail[2].Time", "1646258802617");
            expected.Add("ngx-detail[2].Id", "app-test.user@testing.gov-1646258802617");
            expected.Add("ngx-detail[2].Url", "/queues/my-queue");
            expected.Add("ngx-detail[2].Status", "404");
            expected.Add("ngx-detail[2].Message", "Http failure response for https://localhost:45687/api/customers/1111/images/base64?aspectValue=80&aspectRatio=width&api-version=1.0: 404 OK");
            expected.Add("ngx-detail[2].Stack[0].ColumnNumber", "45");
            expected.Add("ngx-detail[2].Stack[0].LineNumber", "131598");
            expected.Add("ngx-detail[2].Stack[0].FileName", "vendor.js");
            expected.Add("ngx-detail[2].Stack[0].FunctionName", "getValue");
            expected.Add("ngx-detail[2].Stack[0].Source", "foobar");
            expected.Add("ngx-detail[2].Stack[1].ColumnNumber", "75");
            expected.Add("ngx-detail[2].Stack[1].LineNumber", "131998");
            expected.Add("ngx-detail[2].Stack[1].FileName", "vendor.js");
            expected.Add("ngx-detail[2].Stack[1].FunctionName", "setValue");
            expected.Add("ngx-detail[2].Stack[1].Source", "foobar2");


            dict.Should().BeEquivalentTo(expected);
            
        }



        [Theory]
        [InlineData(OltNgxLoggerLevel.Trace, LogEventLevel.Verbose)]
        [InlineData(OltNgxLoggerLevel.Debug, LogEventLevel.Debug)]
        [InlineData(OltNgxLoggerLevel.Information, LogEventLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Log, LogEventLevel.Information)]
        [InlineData(OltNgxLoggerLevel.Warning, LogEventLevel.Warning)]
        [InlineData(OltNgxLoggerLevel.Error, LogEventLevel.Error)]
        [InlineData(OltNgxLoggerLevel.Fatal, LogEventLevel.Fatal)]
        [InlineData(OltNgxLoggerLevel.Off, LogEventLevel.Information)]
        public async Task ForContextTests(OltNgxLoggerLevel level, LogEventLevel expected)
        {
            var model = await FromJsonFile();

            model.Level = level;

            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(_output)
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger())
            {
                logger.Write(model);

                var xyz = TestCorrelator.GetLogEventsFromCurrentContext().First();
                TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.MessageTemplate.Text.Should().Be(OltSerilogConstants.Templates.NgxMessage.Template);
                TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.Level.Should().Be(expected);
                var props = TestCorrelator.GetLogEventsFromCurrentContext().First().Properties;
                props.Count.Should().Be(1);
                props.Should().ContainKey(OltSerilogConstants.Properties.NgxMessage.MessageAsJson);                

                if (model.IsError)
                {
                    TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.Exception.Should().NotBeNull();
                }
                else
                {
                    TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.Exception.Should().BeNull();
                }
            }

        }
    }

}
