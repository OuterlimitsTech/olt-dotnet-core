﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using OLT.AspNetCore.Tests.Assets;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Fact]
        public void ToOltGenericParameter()
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
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);

            results.Values.Should().ContainValues(username, username2, email, email2, userId.ToString());
        }

        [Fact]
        public void ToOltGenericParameterEmpty()
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
        public void ToOltGenericParameterDuplicate()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();

            var formRequest = new Dictionary<string, StringValues>
            {
                { "email", email }
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
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);

            results.Values.Should().ContainValues($"{username},{userId}", email);
        }


        [Fact]
        public void ToOltGenericParameterQuery()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email },
                { "userId", userId.ToString() }
            };

            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);

            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.Query);

            results.Values.Should().ContainValues(username, email, userId.ToString());
        }


        [Fact]
        public void ToOltGenericParameterRouteValues()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email }
            };

            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var routeValues = new RouteValuesFeature();
            routeValues.RouteValues.Add("userId", userId.ToString());


            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            features.Set<IRouteValuesFeature>(routeValues);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.RouteValues);

            results.Values.Should().ContainValues(userId.ToString());
        }


        [Fact]
        public void ToOltGenericParameterForm()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var userId = Faker.RandomNumber.Next();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email },
                { "userId", userId.ToString() }
            };

            var formCollection = new FormCollection(dictionary);
            var form = new FormFeature(formCollection);


            var features = new FeatureCollection();
            features.Set<IFormFeature>(form);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request.Form);            

            results.Values.Should().ContainValues(username, email, userId.ToString());
        }


        [Fact]
        public void OltGenericParameterGetValue()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email }
            };
            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);            
            var value = results.GetValue("username");

            Assert.Equal(value?.ToString(), username);
        }


        [Fact]
        public void OltGenericParameterGetValueUsingGeneric()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email }
            };
            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);

            Assert.Equal(results.GetValue<string>("email"), email);
        }

        [Fact]
        public void OltGenericParameterGetValueUsingGenericNull()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email }
            };
            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);            

            Assert.True(results.GetValue<string>("foobar") == null);
        }

        [Fact]
        public void OltGenericParameterGetValueUsingGenericDefault()
        {
            var username = Faker.Internet.UserName();
            var email = Faker.Internet.Email();
            var defaultEmail = Faker.Internet.Email();
            var dictionary = new Dictionary<string, StringValues>
            {
                { "username", username },
                { "email", email }
            };
            var queryCollection = new QueryCollection(dictionary);
            var query = new QueryFeature(queryCollection);
            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);
            var context = new DefaultHttpContext(features);
            context.Response.Body = new MemoryStream();

            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);

            Assert.Equal(results.GetValue<string>("foobar", defaultEmail), defaultEmail);
        }

        [Fact]
        public void OltGenericParameterEmpty()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();            
            var results = OltHttpRequestExtensions.ToOltGenericParameter(context.Request);
            Assert.False(results.Values.Any());
        }

    }
}
