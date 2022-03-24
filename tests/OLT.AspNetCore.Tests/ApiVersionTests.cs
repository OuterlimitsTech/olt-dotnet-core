using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OLT.Constants;
using OLT.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class ApiVersionTests
    {
        private static string Query = Faker.Name.First();
        private static string MediaType = Faker.Lorem.GetFirstWord();
        private static string Header = Faker.Name.Last();

        public static TheoryData<OltOptionsApiVersionParameter, OltOptionsApiVersionParameter> Data 
        {
            get
            {
                var results = new TheoryData<OltOptionsApiVersionParameter, OltOptionsApiVersionParameter>();
                results.Add(new OltOptionsApiVersionParameter(), new OltOptionsApiVersionParameter());

                results.Add(new OltOptionsApiVersionParameter { Query = Query }, new OltOptionsApiVersionParameter { Query = Query });
                results.Add(new OltOptionsApiVersionParameter { Query = "" }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { Query = " " }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { Query = null }, new OltOptionsApiVersionParameter());

                results.Add(new OltOptionsApiVersionParameter { MediaType = MediaType }, new OltOptionsApiVersionParameter { MediaType = MediaType });
                results.Add(new OltOptionsApiVersionParameter { MediaType = "" }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { MediaType = " " }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { MediaType = null }, new OltOptionsApiVersionParameter());


                results.Add(new OltOptionsApiVersionParameter { Header = Header }, new OltOptionsApiVersionParameter { Header = Header });
                results.Add(new OltOptionsApiVersionParameter { Header = "" }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { Header = " " }, new OltOptionsApiVersionParameter());
                results.Add(new OltOptionsApiVersionParameter { Header = null }, new OltOptionsApiVersionParameter());

                return results;
            }    
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void OltOptionsApiVersionParameterTests(OltOptionsApiVersionParameter options, OltOptionsApiVersionParameter expected)
        {
            var readers = options.BuildReaders();
            readers.Should().HaveCount(4);
            readers.OfType<QueryStringApiVersionReader>().Should().HaveCount(1);
            readers.OfType<MediaTypeApiVersionReader>().Should().HaveCount(1);
            readers.OfType<HeaderApiVersionReader>().Should().HaveCount(1);
            readers.OfType<UrlSegmentApiVersionReader>().Should().HaveCount(1);

            readers.OfType<QueryStringApiVersionReader>().SelectMany(s => s.ParameterNames).Should().HaveCount(1);
            readers.OfType<QueryStringApiVersionReader>().SelectMany(s => s.ParameterNames).FirstOrDefault(p => p.Equals(expected.Query)).Should().NotBeNullOrEmpty();

            readers.OfType<MediaTypeApiVersionReader>().Select(s => s.ParameterName).Should().BeEquivalentTo(expected.MediaType);

            readers.OfType<HeaderApiVersionReader>().SelectMany(s => s.HeaderNames).Should().HaveCount(1);
            readers.OfType<HeaderApiVersionReader>().SelectMany(s => s.HeaderNames).FirstOrDefault(p => p.Equals(expected.Header)).Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void OptionsApiVersionTests()
        {

            Assert.Equal("api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Query);
            Assert.Equal("v", OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType);
            Assert.Equal("x-api-version", OltAspNetCoreDefaults.ApiVersion.ParameterName.Header);


            var model = new OltOptionsApiVersion();
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Query, model.Parameter.Query);
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.MediaType, model.Parameter.MediaType);
            Assert.Equal(OltAspNetCoreDefaults.ApiVersion.ParameterName.Header, model.Parameter.Header); ;
            Assert.True(model.AssumeDefaultVersion);
            model.DefaultVersion.Should().BeEquivalentTo(ApiVersion.Default);

            model.AssumeDefaultVersion = false;

            var version = Faker.Internet.UserName();
            model.Parameter.Query = version;
            model.Parameter.MediaType = version;
            model.Parameter.Header = version;

            Assert.Equal(version, model.Parameter.Query);
            Assert.Equal(version, model.Parameter.MediaType);
            Assert.Equal(version, model.Parameter.Header);
            Assert.False(model.AssumeDefaultVersion);
        }
    }
}
