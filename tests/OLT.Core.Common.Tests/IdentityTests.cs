using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class IdentityTests
    {
        [Fact]
        public void IdentityTestEmpty()
        {
            var model = new TestIdentity();
            Assert.NotNull(model as IOltIdentity);
            Assert.Null(model.Identity);
            Assert.True(model.IsAnonymous);
            Assert.Empty(model.GetAllClaims());
            Assert.Empty(model.GetClaims(ClaimTypes.Name));
            Assert.Empty(model.GetClaims(null));
            Assert.Null(model.GetDbUsername());
            Assert.False(model.HasRole(null));
        }

        [Fact]
        public void IsAnonymousTest()
        {
            var model = new TestIdentity();
            Assert.True(model.IsAnonymous);

            var user = TestHelper.FakerAuthUserToken(Faker.Name.Suffix());
            var identity = new GenericIdentity(user.FullName);
            model = new TestIdentity(identity);
            Assert.True(model.IsAnonymous);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Faker.Internet.UserName()));
            model = new TestIdentity(identity);
            Assert.False(model.IsAnonymous);

        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("Jr.", null)]
        [InlineData("Sr", "555-867-5309")]
        [InlineData(null, "555-867-5309")]
        [InlineData(" ", "")]
        [InlineData("", " ")]
        [InlineData(" ", " ")]
        public void ClaimsTest(string nameSuffix, string phone)
        {
            var user = TestHelper.FakerAuthUserToken(nameSuffix);

            var claims = user.ToClaims();
            claims.AddClaim(ClaimTypes.HomePhone, phone);

            var identity = new GenericIdentity(user.FullName);
            identity.AddClaims(claims);

            var model = new TestIdentity(identity);
            Assert.NotNull(model.Identity);
            Assert.False(model.IsAnonymous);

            TestClaims(user, model, phone);

            Assert.Equal(claims.Count + 1, model.GetAllClaims().Count);  //GenericIdentity adds the ClaimTypes.Name
        }


        [Fact]
        public void ClaimRoleTest()
        {
            var user = TestHelper.FakerAuthUserToken(Faker.Name.Suffix());
            user.Roles.Add(TestSecurityRoles.RoleOne.GetCodeEnum());
            user.Roles.Add(TestSecurityRoles.RoleThree.GetCodeEnum());
            user.Permissions.Add(TestSecurityPermissions.PermTwo.GetCodeEnum());
            var identity = new GenericIdentity(user.FullName);
            identity.AddClaims(user.ToClaims());

            var model = new TestIdentity(identity);

            user.Roles.ForEach(role => Assert.True(model.HasRole(role)));
            user.Permissions.ForEach(perm => Assert.True(model.HasRole(perm)));

            TestClaims(user, model, null);

            Assert.False(model.HasRole(TestSecurityRoles.RoleTwo.GetCodeEnum()));
            Assert.False(model.HasRole(TestSecurityPermissions.PermOne.GetCodeEnum()));
            Assert.False(model.HasRole(null));
            Assert.False(model.HasRole(""));
            Assert.False(model.HasRole(" "));
        }

        [Fact]
        public void ClaimRoleEnumTest()
        {
            var user = TestHelper.FakerAuthUserToken(Faker.Name.Suffix());
            user.Roles.Add(TestSecurityRoles.RoleOne.GetCodeEnum());
            user.Roles.Add(TestSecurityRoles.RoleThree.GetCodeEnum());
            user.Permissions.Add(TestSecurityPermissions.PermTwo.GetCodeEnum());
            var identity = new GenericIdentity(user.FullName);
            identity.AddClaims(user.ToClaims());

            var model = new TestIdentity(identity);

            TestClaims(user, model, null);

            TestSecurityRoles[] nullTest = null;

            Assert.False(model.HasRole(nullTest));

            Assert.True(model.HasRole(TestSecurityRoles.RoleOne));
            Assert.False(model.HasRole(TestSecurityRoles.RoleTwo));
            Assert.True(model.HasRole(TestSecurityRoles.RoleThree));

            Assert.False(model.HasRole(TestSecurityPermissions.PermOne));
            Assert.True(model.HasRole(TestSecurityPermissions.PermTwo));

            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleThree));
            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleOne));
            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleThree, TestSecurityRoles.RoleOne));
        }


        private void TestClaims(OltAuthenticatedUserJson<OltPersonName> user, TestIdentity model, string phone)
        {
            var lastName = string.IsNullOrWhiteSpace(user.Name.Suffix) ? user.Name.Last : $"{user.Name.Last} {user.Name.Suffix}";

            Assert.Equal(user.Name.First, model.FirstName);
            Assert.Equal(user.Name.Middle, model.MiddleName);
            Assert.Equal(lastName, model.LastName);
            Assert.Equal(user.FullName, model.FullName);

            Assert.Equal(user.Username, model.Username);
            Assert.Equal(user.Email, model.Email);
            Assert.Equal(user.UserPrincipalName, model.UserPrincipalName);

            if (!string.IsNullOrWhiteSpace(phone))
            {
                Assert.Equal(phone, model.Phone);
            }
        }

    }
}

