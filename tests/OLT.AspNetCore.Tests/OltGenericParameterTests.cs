using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.AspNetCore.Tests
{
    public class OltGenericParameterTests
    {

        [Fact]
        public void HttpRequestTests()
        {
            var username = Faker.Internet.UserName();
            var username2 = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var email2 = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();

            var formRequest = new Dictionary<string, StringValues>
            {
                { "email", email },
                { "email2", email2 },
            };
            var queryRequest = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "username2", username2 }
            };

            var dictionaries = new List<Dictionary<string, StringValues>>() {  formRequest, queryRequest };
            var expected = dictionaries
                .SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.First());
            expected.Add("userId", userId.ToString());
            expected.Add("userId2", new StringValues());

            var formCollection = new FormCollection(formRequest);
            var form = new FormFeature(formCollection);
            var queryCollection = new QueryCollection(queryRequest);
            var query = new QueryFeature(queryCollection);
            var routeValues = new RouteValuesFeature();
            routeValues.RouteValues.Add("userId", userId.ToString());
            routeValues.RouteValues.Add("userId2", null);

            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            features.Set<IFormFeature>(form);
            features.Set<IRouteValuesFeature>(routeValues);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);
            results.Values.Should().BeEquivalentTo(expected.ToDictionary(v => v.Key, x => x.Value.ToString()));
        }

        [Fact]
        public void EmptyTests()
        {
            OltHttpRequestExtensions.ToOltGenericParameter(new DefaultHttpContext().Request).Values.Should().BeEmpty();

            var formCollection = new FormCollection(new Dictionary<string, StringValues>());
            var form = new FormFeature(formCollection);
            var formFeatures = new FeatureCollection();
            formFeatures.Set<IFormFeature>(form);
            OltHttpRequestExtensions.ToOltGenericParameter(new DefaultHttpContext(formFeatures).Request).Values.Should().BeEmpty();


            var routeValues = new RouteValuesFeature();
            var routeFeatures = new FeatureCollection();
            routeFeatures.Set<IRouteValuesFeature>(routeValues);
            OltHttpRequestExtensions.ToOltGenericParameter(new DefaultHttpContext(routeFeatures).Request).Values.Should().BeEmpty();
        }

        [Fact]
        public void DuplicateTests()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var fullName = Faker.Name.FullName();
            var expected = new StringValues(new string[] { fullName, username, userId.ToString() });

            var formRequest = new Dictionary<string, StringValues>
            {
                { "email", email },
                { "username", fullName }
            };
            var queryRequest = new Dictionary<string, StringValues>
            {
                { "username", username }
            };

            var formCollection = new FormCollection(formRequest);
            var form = new FormFeature(formCollection);
            var queryCollection = new QueryCollection(queryRequest);
            var query = new QueryFeature(queryCollection);
            var routeValues = new RouteValuesFeature();
            routeValues.RouteValues.Add("username", userId.ToString());

            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            features.Set<IFormFeature>(form);
            features.Set<IRouteValuesFeature>(routeValues);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);
            
            results.Values.Should().ContainValues(expected.ToString(), email);
        }


        [Fact]
        public void QueryTests()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var email2 = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "userId", userId.ToString() },
                { "email", new StringValues(new string[] { email2, null, email }) },
            };

            
            var queryCollection = new QueryCollection(dictionary);            
            var query = new QueryFeature(queryCollection);

            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.Query);
            results.Values.Should().BeEquivalentTo(dictionary.ToDictionary(v => v.Key, x => x.Value.ToString()));
        }


        [Fact]
        public void RouteValuesTests()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();

            var expected = new Dictionary<string, string>
            {
                { "username", username },
                { "userId", userId.ToString() },
                { "id", null },
                { "email", email },
            };

            var routeValues = new RouteValuesFeature();
            routeValues.RouteValues.Add("email", email);
            routeValues.RouteValues.Add("userId", userId.ToString());
            routeValues.RouteValues.Add("username", username);
            routeValues.RouteValues.Add("id", null);

            var features = new FeatureCollection();
            features.Set<IRouteValuesFeature>(routeValues);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.RouteValues);

            results.Values.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void ParameterFormTests()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "id", new StringValues(new string[] { null }) },
                { "username", username },
                { "email", email },
                { "userId", userId.ToString() }
            };

            var formCollection = new FormCollection(dictionary);
            var form = new FormFeature(formCollection);

            var features = new FeatureCollection();
            features.Set<IFormFeature>(form);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.Form);
            results.Values.Should().BeEquivalentTo(dictionary.ToDictionary(v => v.Key, x => x.Value.ToString()));
        }


        [Fact]
        public void GetValueTests()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var id = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "id", id.ToString() },
                { "email", email }
            };
            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            //context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);
            
            Assert.Equal(username, results.GetValue("username")?.ToString());
            Assert.Equal(email, results.GetValue<string>("email"));
            Assert.Equal(id, results.GetValue<int>("id"));
            Assert.Equal(0, results.GetValue<int>("email"));
            Assert.Null(results.GetValue<string>("foobar"));
            Assert.Equal(0, results.GetValue<int>("foobar"));

            var defaultValue = Faker.Internet.Email();
            Assert.Equal(defaultValue, results.GetValue<string>("foobar", defaultValue));
            Assert.Equal(1000, results.GetValue<int>("foobar", 1000));
        }



        [Fact]
        public async Task ExceptionTests()
        {
            HttpRequest httpContext = null;
            Assert.Throws<ArgumentNullException>("request", () => OltHttpRequestExtensions.ToOltGenericParameter(httpContext));
            await Assert.ThrowsAsync<ArgumentNullException>("request", () => OltHttpRequestExtensions.GetRawBodyStringAsync(httpContext, null));
            await Assert.ThrowsAsync<ArgumentNullException>("request", () => OltHttpRequestExtensions.GetRawBodyBytesAsync(httpContext));

            RouteValueDictionary routeValue = null;
            QueryCollection queryCollection = null;
            FormCollection formCollection = null;
            Assert.Throws<ArgumentNullException>("value", () => OltHttpRequestExtensions.ToOltGenericParameter(routeValue));
            Assert.Throws<ArgumentNullException>("value", () => OltHttpRequestExtensions.ToOltGenericParameter(queryCollection));
            Assert.Throws<ArgumentNullException>("value", () => OltHttpRequestExtensions.ToOltGenericParameter(formCollection));

        }
    }
}
