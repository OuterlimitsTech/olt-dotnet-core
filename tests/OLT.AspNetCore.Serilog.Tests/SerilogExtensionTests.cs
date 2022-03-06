using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OLT.AspNetCore.Serilog.Tests.Assets;
using OLT.AspNetCore.Serilog.Tests.Assets.Startups;
using OLT.Constants;
using OLT.Core;
using OLT.Logging.Serilog;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Serilog.Tests
{
    public class SerilogServerTests
    {

        [Fact]
        public async Task MiddlewareSession()
        {
            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<StartupMiddleware>()))
            {
                using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                {
                    Log.Logger = logger;
                    var identity = testServer.Services.GetService<IOltIdentity>();

                    var response = await testServer.CreateRequest("/api").SendAsync("GET");
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
                    TestIdentityProperties(logs.First().Properties, identity);
                }
            }
        }

        [Fact]
        public async Task MiddlewareThrowError()
        {

            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<StartupMiddleware>()))
            {
                using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                {
                    Log.Logger = logger;
                    var identity = testServer.Services.GetService<IOltIdentity>();

                    var request = testServer.CreateRequest("/api/throw-error");
                    request.AddHeader("header-one", "value-one");
                    var response = await request.GetAsync();
                    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                    var body = await response.Content.ReadAsStringAsync();
                    
                    var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
                    logs.Should().HaveCount(2);

                    var json = JsonConvert.DeserializeObject<OltErrorHttpSerilog>(body);
                    var serverError = logs.First(p => p.MessageTemplate.Text == OltSerilogConstants.Templates.AspNetCore.ServerError);
                    var payload = logs.First(p => p.MessageTemplate.Text == OltSerilogConstants.Templates.AspNetCore.Payload);

                    Assert.NotNull(json);
                    Assert.NotNull(serverError);
                    Assert.NotNull(payload);

                    ValidatePayloadProperties(payload.Properties);
                    TestIdentityProperties(payload.Properties, identity);
                    TestIdentityProperties(serverError.Properties, identity);
                    ValidateAppRequestUid(json, serverError);
                    ValidateAppRequestUid(json, payload);

                    CleanValue(payload.Properties[OltSerilogConstants.Properties.AspNetCore.ResponseBody]).Should().Contain(json.ErrorUid.ToString());
                    CleanValue(payload.Properties[OltSerilogConstants.Properties.AspNetCore.RequestHeaders]).Should().Contain("header-one");                   

                }
            }
        }

        [Fact]
        public async Task MiddlewareBadRequest()
        {

            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<StartupMiddleware>()))
            {
                using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                {
                    Log.Logger = logger;
                    var identity = testServer.Services.GetService<IOltIdentity>();

                    var request = testServer.CreateRequest("/api/bad-request");
                    var response = await request.GetAsync();
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    var body = await response.Content.ReadAsStringAsync();

                    var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
                    logs.Should().HaveCount(1);

                    var json = JsonConvert.DeserializeObject<OltErrorHttpSerilog>(body);
                    var payload = logs.First(p => p.MessageTemplate.Text == OltSerilogConstants.Templates.AspNetCore.Payload);

                    
                    Assert.NotNull(json);
                    Assert.NotNull(payload);
                    Assert.Equal("bad-request", json.Message);

                    ValidatePayloadProperties(payload.Properties);
                    TestIdentityProperties(payload.Properties, identity);
                    ValidateAppRequestUid(json, payload);

                    CleanValue(payload.Properties[OltSerilogConstants.Properties.AspNetCore.ResponseBody]).Should().Contain(json.ErrorUid.ToString());

                }
            }
        }

        [Fact]
        public async Task MiddlewareValidationError()
        {

            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<StartupMiddleware>()))
            {
                using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                {
                    Log.Logger = logger;
                    var identity = testServer.Services.GetService<IOltIdentity>();

                    var request = testServer.CreateRequest("/api/validation-error");
                    var response = await request.GetAsync();
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    var body = await response.Content.ReadAsStringAsync();

                    var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
                    logs.Should().HaveCount(1);

                    var json = JsonConvert.DeserializeObject<OltErrorHttpSerilog>(body);
                    var payload = logs.First(p => p.MessageTemplate.Text == OltSerilogConstants.Templates.AspNetCore.Payload);


                    Assert.NotNull(json);
                    Assert.NotNull(payload);
                    Assert.Equal("Please correct the validation errors", json.Message);

                    ValidatePayloadProperties(payload.Properties);
                    TestIdentityProperties(payload.Properties, identity);
                    ValidateAppRequestUid(json, payload);

                    CleanValue(payload.Properties[OltSerilogConstants.Properties.AspNetCore.ResponseBody]).Should().Contain(json.ErrorUid.ToString());

                }
            }
        }


        [Fact]
        public async Task MiddlewareRecordNotFound()
        {

            using (var testServer = new TestServer(TestHostBuilder.WebHostBuilder<StartupMiddleware>()))
            {
                using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                {
                    Log.Logger = logger;
                    var identity = testServer.Services.GetService<IOltIdentity>();

                    var request = testServer.CreateRequest("/api/record-not-found");
                    var response = await request.GetAsync();
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    var body = await response.Content.ReadAsStringAsync();

                    var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToList();
                    logs.Should().HaveCount(1);

                    var json = JsonConvert.DeserializeObject<OltErrorHttpSerilog>(body);
                    var payload = logs.First(p => p.MessageTemplate.Text == OltSerilogConstants.Templates.AspNetCore.Payload);


                    Assert.NotNull(json);
                    Assert.NotNull(payload);
                    Assert.Equal("record-not-found", json.Message);

                    ValidatePayloadProperties(payload.Properties);
                    TestIdentityProperties(payload.Properties, identity);
                    ValidateAppRequestUid(json, payload);

                    CleanValue(payload.Properties[OltSerilogConstants.Properties.AspNetCore.ResponseBody]).Should().Contain(json.ErrorUid.ToString());

                }
            }
        }

        
        private static void ValidateAppRequestUid(OltErrorHttpSerilog json, LogEvent @event)
        {
            var appRequestUid = CleanValue(@event.Properties[OltSerilogConstants.Properties.AspNetCore.AppRequestUid]).ToGuid();
            Assert.Equal(json.ErrorUid, appRequestUid);
        }

        private static void ValidatePayloadProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.AppRequestUid).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.RequestHeaders).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.ResponseHeaders).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.RequestBody).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.ResponseBody).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.AspNetCore.RequestUri).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.UserPrincipalName).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.Username).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.DbUsername).Should().BeTrue();
            //Assert.Equal(identity.UserPrincipalName, CleanValue(properties[OltSerilogConstants.Properties.UserPrincipalName]));
            //Assert.Equal(identity.Username, CleanValue(properties[OltSerilogConstants.Properties.Username]));
            //Assert.Equal(identity.Username, CleanValue(properties[OltSerilogConstants.Properties.DbUsername]));
        }

        private static void TestIdentityProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties, IOltIdentity identity)
        {
            properties.ContainsKey(OltSerilogConstants.Properties.UserPrincipalName).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.Username).Should().BeTrue();
            properties.ContainsKey(OltSerilogConstants.Properties.DbUsername).Should().BeTrue();
            Assert.Equal(identity.UserPrincipalName, CleanValue(properties[OltSerilogConstants.Properties.UserPrincipalName]));
            Assert.Equal(identity.Username, CleanValue(properties[OltSerilogConstants.Properties.Username]));
            Assert.Equal(identity.Username, CleanValue(properties[OltSerilogConstants.Properties.DbUsername]));
        }

        private static string CleanValue(LogEventPropertyValue value)
        {
            return value.ToString().Replace("\"", string.Empty);
        }

        //testServer.BaseAddress = new Uri("https://example.com/A/Path/");

        //var context = await testServer.SendAsync(c =>
        //{
        //    c.Request.Method = HttpMethods.Post;
        //    c.Request.Path = "/and/file.txt";
        //    c.Request.QueryString = new QueryString("?and=query");
        //});



        [Fact]
        public void ServiceArgumentExceptions()
        {
            var services = new ServiceCollection();
            Action<OltSerilogOptions> action = (OltSerilogOptions opts) =>
            {
                opts.ErrorMessage = "Test";
                opts.ShowExceptionDetails = true;
            };

            Assert.Throws<ArgumentNullException>("services", () => OltSerilogAspNetCoreExtensions.AddOltSerilog(null, null));

            try
            {
                OltSerilogAspNetCoreExtensions.AddOltSerilog(services, null);
                OltSerilogAspNetCoreExtensions.AddOltSerilog(services, action);
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }

        }

        [Fact]
        public void AppArgumentExceptions()
        {
            var services = new ServiceCollection();

            Action<RequestLoggingOptions> action = (RequestLoggingOptions opts) =>
            {
                opts.MessageTemplate = "Test";
            };

            var app = new ApplicationBuilder(services.BuildServiceProvider());
            Assert.Throws<ArgumentNullException>("app", () => OltSerilogAspNetCoreExtensions.UseOltSerilogRequestLogging(null, null));


            try
            {
                OltSerilogAspNetCoreExtensions.UseOltSerilogRequestLogging(app, null);
                OltSerilogAspNetCoreExtensions.UseOltSerilogRequestLogging(app, action);
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }
        }
    }
}
