using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.Logging.Serilog.Tests.NgxLogger
{
    public class NgxLoggerDetailMessageTests
    {

        public static IEnumerable<object[]> NgxLoggerDetailData =>
            new List<object[]>
            {
                new object[] { new HelperNgxExceptionTest(DateTimeOffset.Now) },
                new object[] { new HelperNgxExceptionTest(null) },
            };

        [Theory]
        [MemberData(nameof(NgxLoggerDetailData))]
        public void MessageTest(HelperNgxExceptionTest data)
        {
            var exception = data.Detail.ToException();
            Assert.Equal(data.Detail.Message, exception.Message);
            Assert.Equal(data.Detail.Id, exception.Source);
            var dict = TestHelper.ToDictionary(exception.Data);

            Assert.Collection(dict,
                item => Assert.Equal(data.Result["Name"], item.Value),
                item => Assert.Equal(data.Result["AppId"], item.Value),
                item => Assert.Equal(data.Result["User"], item.Value),
                item => Assert.Equal(data.Result["Time"], item.Value),
                item => Assert.Equal(data.Result["Url"], item.Value),
                item => Assert.Equal(data.Result["Status"], item.Value),
                item => Assert.Equal(data.Result["Stack"], item.Value)
            );

        }
    }
}
