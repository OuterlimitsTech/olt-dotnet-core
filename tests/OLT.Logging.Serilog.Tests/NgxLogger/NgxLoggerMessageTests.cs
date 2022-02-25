using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{

    public class NgxLoggerMessageTests
    {

        [Theory]
        [MemberData(nameof(NgxLoggerMessageData))]
        public void MessageTest(OltNgxLoggerLevel? level, bool loadDetail, HelperNgxExceptionTest data)
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
                    item => Assert.Equal(data.Result["LineNumber"], item.Value),
                    item => Assert.Equal(data.Result["FileName"], item.Value),
                    item => Assert.Equal(data.Result["Timestamp"], item.Value)
                );

                return;
            }


            Assert.Collection(dict,
                item => Assert.Equal(data.Result["Username"], item.Value),
                item => Assert.Equal(data.Result["Level"], item.Value),
                item => Assert.Equal(data.Result["LineNumber"], item.Value),
                item => Assert.Equal(data.Result["FileName"], item.Value),
                item => Assert.Equal(data.Result["Timestamp"], item.Value)
            );

        }


        public static IEnumerable<object[]> NgxLoggerMessageData =>
         new List<object[]>
         {
                new object[] { OltNgxLoggerLevel.Error, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Fatal, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Info, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, true, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Trace, true, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Warn, true, new HelperNgxExceptionTest(null) },

                new object[] { OltNgxLoggerLevel.Error, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Fatal, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { OltNgxLoggerLevel.Warn, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, false, new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { null, false, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Debug, false, new HelperNgxExceptionTest(null) },
                new object[] { OltNgxLoggerLevel.Log, false, new HelperNgxExceptionTest(null) },
         };

    }
}
