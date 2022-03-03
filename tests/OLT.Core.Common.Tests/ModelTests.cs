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
            var list = new List<TestPersonModel>();
            for (int i = 1; i <= Faker.RandomNumber.Next(18, 134); i++)
            {
                list.Add(new TestPersonModel
                {
                    Name = Faker.Lorem.Words(i).Last(),
                    StreetAddress = Faker.Lorem.Paragraphs(i).Last()
                });
            }

            var criteria = new TestCriteriaModel
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            
            var model = new OltPagedSearchJson<TestPersonModel, TestCriteriaModel>();
            Assert.Equal(0, model.Page);
            Assert.Equal(0, model.Size);
            Assert.Equal(0, model.Count);
            Assert.Null(model.Criteria);
            Assert.Null(model.Data);
            Assert.False(model.Asc);

            Assert.Null(model as IOltPagingParams);
            Assert.NotNull(model as IOltPaged);
            Assert.NotNull(model as IOltPaged<TestPersonModel>);
            Assert.NotNull(model as OltPagedJson<TestPersonModel>);
            
            var page = Faker.RandomNumber.Next();
            var size = Faker.RandomNumber.Next();
            var count = Faker.RandomNumber.Next();
            var sortBy = Faker.Lorem.Words(10).Last();

            model.Criteria = criteria;
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
            model.Criteria.Should().BeEquivalentTo(criteria);
            model.Data.Should().BeEquivalentTo(list);
        }

        [Fact]
        public void OltAuthenticatedUserTokenJsonTest()
        {
            var model = new OltAuthenticatedUserTokenJson<OltPersonName>();
            Assert.Equal(0, model.UserPrincipalName);
            Assert.NotNull(model.Name);
            Assert.NotNull(model.Name as IOltPersonName);
            Assert.Empty(model.Roles);
            Assert.Empty(model.Permissions);

            var name = new OltPersonName
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
                Suffix = Faker.Name.Suffix(),
            };

            var roles = new List<string>();
            var permissions = new List<string>();
            for (int i = 1; i <= Faker.RandomNumber.Next(8, 13); i++)
            {
                roles.Add(Faker.Lorem.Words(i).Last());
            }

            for (int i = 50; i <= Faker.RandomNumber.Next(58, 79); i++)
            {
                permissions.Add(Faker.Lorem.Words(i).Last());
            }


            var upn = Faker.RandomNumber.Next();
            var userName = Faker.Internet.UserName();
            var emailAddress = Faker.Internet.Email();
            var authenticationType = Faker.Internet.DomainWord();
            var token = Faker.Lorem.Words(8).Last();
            var issued = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(2, 10));
            var expires = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(20, 40));

            model.UserPrincipalName = upn;
            model.Username = userName;
            model.EmailAddress = emailAddress;
            model.Name = name;
            model.AuthenticationType = authenticationType;
            model.Token = token;
            model.Issued = issued;
            model.Expires = expires;
            model.Roles = roles;
            model.Permissions = permissions;
            

            Assert.Equal(upn, model.UserPrincipalName);
            Assert.Equal(userName, model.Username);
            Assert.Equal(emailAddress, model.EmailAddress);
            Assert.Equal(token, model.Token);
            Assert.Equal(issued, model.Issued);
            Assert.Equal(expires, model.Expires);
            Assert.Equal(name.FullName, model.FullName);
            Assert.Equal($"{(expires - issued).TotalSeconds}", model.ExpiresIn);



            model.Name.Should().BeEquivalentTo(name);
            model.Roles.Should().BeEquivalentTo(roles);
            model.Permissions.Should().BeEquivalentTo(permissions);

        }

    }
}

