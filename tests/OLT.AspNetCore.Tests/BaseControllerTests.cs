using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;
using System.Net;
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
        }

        [Fact]
        public async Task InternalServerErrorTests()
        {
            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var value = Faker.Name.First();
                var request = testServer.CreateRequest($"/api/throw-error?value={value}");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                Assert.Equal($"\"{value}\"", body);
            }

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var value = new OltErrorHttp
                {
                    Message = Faker.Name.First()
                };

                var request = testServer.CreateRequest($"/api/bad-request?value={value.Message}");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);
                result.Should().BeEquivalentTo(value);
            }
        }

        [Fact]
        public async Task BadRequestTests()
        {

            using (var testServer = new TestServer(TestHelper.WebHostBuilder<Startup>()))
            {
                var value = new OltErrorHttp
                {
                    Message = Faker.Name.First()
                };

                var request = testServer.CreateRequest($"/api/bad-request?value={value.Message}");
                var response = await request.SendAsync("GET");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OltErrorHttp>(body);
                result.Should().BeEquivalentTo(value);
            }
        }
    }
}