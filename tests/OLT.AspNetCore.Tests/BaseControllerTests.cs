using AwesomeAssertions;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using OLT.AspNetCore.Tests.Assets;
using OLT.Constants;
using OLT.Core;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class BaseControllerTests
    {

        [Fact]
        public async Task OkTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var response = await testServer.CreateRequest("/api").SendAsync("GET");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var expected = new OltErrorHttp
                {
                    Message = Faker.Name.First(),
                    Errors = new System.Collections.Generic.List<string>
                    {
                        Faker.Name.Last(),
                        Faker.Address.StreetAddress()
                    }
                };

                using (var client = testServer.CreateClient())
                {
                    var response = await client.PostAsync("/api", new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8,  "application/json"));
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);
                    result.Should().BeEquivalentTo(expected);

                }

            }
        }

        [Fact]
        public async Task InternalServerErrorTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var expected = new OltErrorHttp
                {
                    Message = Faker.Name.First()
                };
                var request = testServer.CreateRequest($"/api/throw-error?value={expected.Message}");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);                
                result.Should().BeEquivalentTo(expected);
            }

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var expected = new OltErrorHttp
                {
                    Message = OltAspNetCoreDefaults.InternalServerMessage
                };

                var request = testServer.CreateRequest($"/api/throw-error");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);
                result.Should().BeEquivalentTo(expected);
            }

        }

        [Fact]
        public async Task BadRequestTests()
        {

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var expected = new OltErrorHttp
                {
                    Message = Faker.Name.First()
                };

                var request = testServer.CreateRequest($"/api/bad-request?value={expected.Message}");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);
                result.Should().BeEquivalentTo(expected);
            }
        }
    }
}