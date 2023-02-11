//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.ApiExplorer;
//using Asp.Versioning;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.DependencyInjection;
//using OLT.AspNetCore.Tests.Assets;
//using OLT.Constants;
//using OLT.Core;
//using System;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace OLT.AspNetCore.Tests
//{

//    public class ApiVersionTests
//    {
//        private static string Query = Faker.Name.First();
//        private static string MediaType = Faker.Lorem.GetFirstWord();
//        private static string Header = Faker.Name.Last();


//        [Theory]
//        [InlineData("/api/api-version/one", HttpStatusCode.OK)]
//        [InlineData("/api/api-version/one?api-version=1.0", HttpStatusCode.OK)]
//        [InlineData("/api/api-version/one?api-version=2.0", HttpStatusCode.OK)]
//        [InlineData("/api/api-version/one?api-version=3.0", HttpStatusCode.BadRequest)]
//        [InlineData("/api/api-version/two", HttpStatusCode.BadRequest)]
//        [InlineData("/api/api-version/two?api-version=1.0", HttpStatusCode.BadRequest)]
//        [InlineData("/api/api-version/two?api-version=2.0", HttpStatusCode.OK)]
//        [InlineData("/api/api-version/two?api-version=3.0", HttpStatusCode.BadRequest)]
//        public async Task ControllerTests(string uri, HttpStatusCode expected)
//        {
//            using (var testServer = new TestServer(TestHelper.WebHostBuilder<StartupWithApiVersion>()))
//            {
//                using (var client = testServer.CreateClient())
//                {
//                    var response = await client.GetAsync(uri);
//                    Assert.Equal(expected, response.StatusCode);

//                    var provider = testServer.Services.GetService<IApiDescriptionGroupCollectionProvider>();
//                    Assert.NotNull(provider);
//                    provider.ApiDescriptionGroups.Items.Should().HaveCount(2);                    
//                }
//            }
//        }

//        public static TheoryData<OltOptionsApiVersionParameter, OltOptionsApiVersionParameter> Data 
//        {
//            get
//            {
//                var results = new TheoryData<OltOptionsApiVersionParameter, OltOptionsApiVersionParameter>();
//                results.Add(new OltOptionsApiVersionParameter(), new OltOptionsApiVersionParameter());

//                results.Add(new OltOptionsApiVersionParameter { Query = Query }, new OltOptionsApiVersionParameter { Query = Query });
//                results.Add(new OltOptionsApiVersionParameter { Query = "" }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { Query = " " }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { Query = null }, new OltOptionsApiVersionParameter());

//                results.Add(new OltOptionsApiVersionParameter { MediaType = MediaType }, new OltOptionsApiVersionParameter { MediaType = MediaType });
//                results.Add(new OltOptionsApiVersionParameter { MediaType = "" }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { MediaType = " " }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { MediaType = null }, new OltOptionsApiVersionParameter());


//                results.Add(new OltOptionsApiVersionParameter { Header = Header }, new OltOptionsApiVersionParameter { Header = Header });
//                results.Add(new OltOptionsApiVersionParameter { Header = "" }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { Header = " " }, new OltOptionsApiVersionParameter());
//                results.Add(new OltOptionsApiVersionParameter { Header = null }, new OltOptionsApiVersionParameter());

//                return results;
//            }    
//        }


//        [Theory]
//        [MemberData(nameof(Data))]
//        public void OltOptionsApiVersionParameterTests(OltOptionsApiVersionParameter options, OltOptionsApiVersionParameter expected)
//        {
//            var readers = options.BuildReaders();
//            readers.Should().HaveCount(4);
//            readers.OfType<QueryStringApiVersionReader>().Should().HaveCount(1);
//            readers.OfType<MediaTypeApiVersionReader>().Should().HaveCount(1);
//            readers.OfType<HeaderApiVersionReader>().Should().HaveCount(1);
//            readers.OfType<UrlSegmentApiVersionReader>().Should().HaveCount(1);

//            readers.OfType<QueryStringApiVersionReader>().SelectMany(s => s.ParameterNames).Should().HaveCount(1);
//            readers.OfType<QueryStringApiVersionReader>().SelectMany(s => s.ParameterNames).FirstOrDefault(p => p.Equals(expected.Query)).Should().NotBeNullOrEmpty();

//            readers.OfType<MediaTypeApiVersionReader>().Select(s => s.ParameterName).Should().BeEquivalentTo(expected.MediaType);

//            readers.OfType<HeaderApiVersionReader>().SelectMany(s => s.HeaderNames).Should().HaveCount(1);
//            readers.OfType<HeaderApiVersionReader>().SelectMany(s => s.HeaderNames).FirstOrDefault(p => p.Equals(expected.Header)).Should().NotBeNullOrEmpty();

//        }

//        [Fact]
//        public void OptionsApiVersionTests()
//        {
//            Assert.Equal("api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Query);
//            Assert.Equal("v", OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType);
//            Assert.Equal("x-api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Header);

//            var model = new OltOptionsApiVersion();
//            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Query, model.Parameter.Query);
//            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType, model.Parameter.MediaType);
//            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Header, model.Parameter.Header); ;
//            Assert.True(model.AssumeDefaultVersion);
//            model.DefaultVersion.Should().BeEquivalentTo(ApiVersion.Default);

//            model.AssumeDefaultVersion = false;

//            var queryVersion = Faker.Internet.UserName();
//            var mediaVersion = Faker.Internet.DomainName();
//            var headerVersion = Faker.Name.First();
//            model.Parameter.Query = queryVersion;
//            model.Parameter.MediaType = mediaVersion;
//            model.Parameter.Header = headerVersion;

//            Assert.Equal(queryVersion, model.Parameter.Query);
//            Assert.Equal(mediaVersion, model.Parameter.MediaType);
//            Assert.Equal(headerVersion, model.Parameter.Header);
//            Assert.False(model.AssumeDefaultVersion);
//        }

//        [Fact]
//        public void ArgumentExceptions()
//        {
//            var services = new ServiceCollection();
//            OltOptionsApiVersion nullOptions = null;
//            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddApiVersioning(null, nullOptions));
//            Assert.Throws<ArgumentNullException>("options", () => OltServiceCollectionAspnetCoreExtensions.AddApiVersioning(services, nullOptions));
//            Assert.Throws<ArgumentNullException>("services", () => OltServiceCollectionAspnetCoreExtensions.AddApiVersioning(null, new OltOptionsApiVersion()));
//        }
//    }
//}
