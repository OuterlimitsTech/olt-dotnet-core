using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class ModelTests
    {

        [Fact]
        public void FileBase64Tests()
        {
            var model = new OltFileBase64();
            Assert.Null(model.FileBase64);
            Assert.Null(model.FileName);
            Assert.Null(model.ContentType);
            Assert.False(model.Success);

            Assert.NotNull(model as IOltFileBase64);

            var fileName = Faker.Internet.DomainName();
            var contentType = Faker.Internet.DomainWord();
            var fileBase64 = Faker.Lorem.Paragraph();


            model.FileName = fileName;
            model.ContentType = contentType;
            Assert.False(model.Success);

            model.FileBase64 = "";
            Assert.False(model.Success);

            model.FileBase64 = " ";
            Assert.False(model.Success);

            model.FileBase64 = fileBase64;
            Assert.True(model.Success);

            Assert.Equal(fileName, model.FileName);
            Assert.Equal(contentType, model.ContentType);
            Assert.Equal(fileBase64, model.FileBase64);
        }


        [Theory]
        [InlineData("Test Jones", "Test", null, "Jones", null)]
        [InlineData("Test M Jones", "Test", "M", "Jones", null)]
        [InlineData("Test M Jones Jr", "Test", "M", "Jones", "Jr")]
        [InlineData("M Jones Jr", null, "M", "Jones", "Jr")]
        [InlineData("M Jr", null, "M", null, "Jr")]
        [InlineData("", null, null, null, null)]
        public void PersonNameTest(string expected, string first, string middle, string last, string suffix)
        {
            var model = new OltPersonName
            {
                First = first,
                Middle = middle,
                Last = last,
                Suffix = suffix
            };            

            Assert.Equal(first, model.First);
            Assert.Equal(middle, model.Middle);
            Assert.Equal(last, model.Last);
            Assert.Equal(suffix, model.Suffix);
            Assert.Equal(expected, model.FullName);
            Assert.NotNull(model as IOltPersonName);
        }


        [Fact]
        public void PagingParamsTest()
        {
            var model = new OltPagingParams();
            Assert.Equal(1, model.Page);
            Assert.Equal(10, model.Size);
            Assert.NotNull(model as IOltPagingParams);
            Assert.NotNull(model as IOltPaged);            

            var page = Faker.RandomNumber.Next();
            var size = Faker.RandomNumber.Next();
            model.Page = page;
            model.Size = size;

            Assert.Equal(page, model.Page);
            Assert.Equal(size, model.Size);            
        }

        [Fact]
        public void OltSortParamsTest()
        {
            var model = new OltSortParams();
            Assert.False(model.IsAscending);
            Assert.Null(model.PropertyName);
            Assert.NotNull(model as IOltSortParams);

            var propName = Faker.Lorem.Words(11).Last();
            
            model.PropertyName = propName;
            model.IsAscending = true;

            Assert.Equal(propName, model.PropertyName);
            Assert.True(model.IsAscending);

        }

        [Fact]
        public void OltPagedJsonTest()
        {
            var list = new List<TestPersonModel>();
            for(int i = 1; i <= Faker.RandomNumber.Next(18, 134); i++)
            {
                list.Add(new TestPersonModel 
                { 
                    Name = Faker.Lorem.Words(i).Last(), 
                    StreetAddress = Faker.Lorem.Paragraphs(i).Last() 
                });
            }

            var model = new OltPagedJson<TestPersonModel>();
            Assert.Equal(0, model.Page);
            Assert.Equal(0, model.Size);
            Assert.Equal(0, model.Count);
            Assert.Null(model.Data);
            Assert.False(model.Asc);

            Assert.Null(model as IOltPagingParams);
            Assert.NotNull(model as IOltPaged);
            Assert.NotNull(model as IOltPaged<TestPersonModel>);

            var page = Faker.RandomNumber.Next();
            var size = Faker.RandomNumber.Next();
            var count = Faker.RandomNumber.Next();
            var sortBy = Faker.Lorem.Words(10).Last();
            
            model.Page = page;
            model.Size = size;
            model.Count = count;
            model.SortBy = sortBy;
            model.Data = list;
            model.Asc = true;

            Assert.Equal(page, model.Page);
            Assert.Equal(size, model.Size);
            Assert.Equal(count, model.Count);
            Assert.Equal(sortBy, model.SortBy);
            Assert.True(model.Asc);
            model.Data.Should().BeEquivalentTo(list);
        }

        [Fact]
        public void OltPagedSearchJsonTest()
        {
            var criteria = new TestCriteriaModel
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            var key = Guid.NewGuid().ToString();

            var model = new OltPagedSearchJson<TestPersonModel, TestCriteriaModel>();
            Assert.Null(model.Criteria);
            Assert.Null(model.Data);
            Assert.Null(model.Key);
            Assert.Null(model as IOltPagingParams);
            Assert.NotNull(model as IOltPaged);
            Assert.NotNull(model as IOltPaged<TestPersonModel>);
            Assert.NotNull(model as OltPagedJson<TestPersonModel>);

            model.Key = key;
            model.Criteria = criteria;            
            model.Criteria.Should().BeEquivalentTo(criteria);
            Assert.Equal(key, model.Key);
        }

        [Fact]
        public void OltAuthenticatedUserTokenJsonTestEmpty()
        {
            var model = new OltAuthenticatedUserJwtTokenJson<OltPersonName>();
            Assert.NotNull(model.Name);
            Assert.NotNull(model.Name as IOltPersonName);
            Assert.NotNull(model as OltAuthenticatedUserJson<OltPersonName>);

            Assert.Null(model.UserPrincipalName);
            Assert.Null(model.Username);
            Assert.Null(model.Email);
            Assert.Equal("", model.FullName);
            Assert.Null(model.AuthenticationType);
            Assert.Null(model.Token);
            Assert.Null(model.Issued);
            Assert.Null(model.Expires);
            Assert.Null(model.ExpiresIn);
            Assert.Empty(model.Roles);
            Assert.Empty(model.Permissions);

        }

        [Fact]
        public void OltAuthenticatedUserTokenJsonTest()
        {    
            var name = TestHelper.FakerPersonName(Faker.Name.Suffix());
            var roles = TestHelper.FakerRoleList("role-1", 8, 13);
            var permissions = TestHelper.FakerRoleList("perm-", 10, 25);

            var upn = Faker.RandomNumber.Next().ToString();
            var userName = Faker.Internet.UserName();
            var emailAddress = Faker.Internet.Email();
            var authenticationType = Faker.Internet.DomainWord();
            var token = Faker.Lorem.Words(8).Last();
            var issued = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(2, 10));
            var expires = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(20, 40));

            var model = new OltAuthenticatedUserJwtTokenJson<OltPersonName>();
            model.UserPrincipalName = upn;
            model.Username = userName;
            model.Email = emailAddress;
            model.Name = name;
            model.AuthenticationType = authenticationType;
            model.Token = token;
            model.Issued = issued;
            model.Expires = expires;
            model.Roles = roles;
            model.Permissions = permissions;
            

            Assert.Equal(upn, model.UserPrincipalName);
            Assert.Equal(userName, model.Username);
            Assert.Equal(emailAddress, model.Email);
            Assert.Equal(token, model.Token);
            Assert.Equal(issued, model.Issued);
            Assert.Equal(expires, model.Expires);
            Assert.Equal(name.FullName, model.FullName);
            Assert.Equal((expires - issued).TotalSeconds, model.ExpiresIn);



            model.Name.Should().BeEquivalentTo(name);
            model.Roles.Should().BeEquivalentTo(roles);
            model.Permissions.Should().BeEquivalentTo(permissions);

        }

    }
}

