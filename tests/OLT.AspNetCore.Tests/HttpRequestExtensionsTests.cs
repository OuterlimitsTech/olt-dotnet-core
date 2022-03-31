using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class HttpRequestExtensionsTests
    {

        [Fact]
        public async Task GetRawBodyStringAsync()
        {
            var dto = PersonDto.FakerData();
            var json = JsonConvert.SerializeObject(dto);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpContext = new DefaultHttpContext()
            {
                Request = { Body = stream, ContentLength = stream.Length }
            };

            var result = await OltHttpRequestExtensions.GetRawBodyStringAsync(httpContext.Request);
            Assert.True(result.Equals(json));
        }


        [Fact]
        public async Task GetRawBodyBytesAsync()
        {
            var dto = PersonDto.FakerData();
            var json = JsonConvert.SerializeObject(dto);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpContext = new DefaultHttpContext()
            {
                Request = { Body = stream, ContentLength = stream.Length }
            };

            var result = await OltHttpRequestExtensions.GetRawBodyBytesAsync(httpContext.Request);

            Assert.True(result.Length.Equals(json.ToASCIIBytes().Length));
        }

    }
}
