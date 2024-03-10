using OLT.Constants;
using OLT.Core.Common.Tests.Assets;
using OLT.Core.Common.Tests.Assets.Identity;
using System.Security.Principal;
using Xunit;

namespace OLT.Core.Common.Tests;

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
            Assert.Empty(model.GetClaims(OltClaimTypes.Name));
            Assert.Empty(model.GetClaims(null));
            Assert.Null(model.GetDbUsername());
            Assert.False(model.HasRole(null));


            Assert.Null(model.Username);
            Assert.Null(model.FirstName);
            Assert.Null(model.MiddleName);
            Assert.Null(model.LastName);
            Assert.Null(model.Email);
            Assert.Null(model.Phone);
            Assert.Null(model.FullName);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Null(model.UserPrincipalName);
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.Null(model.NameId);
            Assert.Null(model.Subject);
            Assert.Null(model.PhoneVerified);
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


            identity.AddClaim(new System.Security.Claims.Claim(OltClaimTypes.PreferredUsername, Faker.Internet.UserName()));
            model = new TestIdentity(identity);
            Assert.False(model.IsAnonymous);
        }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("Jr.", null, null)]
    [InlineData("Jr.", null, false)]
    [InlineData("Sr", "555-867-5309", true)]
    [InlineData(null, "555-867-5309", true)]
    [InlineData(" ", "", false)]
    [InlineData("", " ", false)]
    [InlineData(" ", " ", false)]
    public void ClaimsTest(string nameSuffix, string phone, bool? phoneVerified)
    {
            var user = TestHelper.FakerAuthUserToken(nameSuffix);

            var claims = user.ToClaims();
            claims.AddClaim(OltClaimTypes.PhoneNumber, phone);
            claims.AddClaim(OltClaimTypes.PhoneNumberVerified, phoneVerified?.ToString());

            var identity = new GenericIdentity(user.FullName);
            identity.AddClaims(claims);

            var model = new TestIdentity(identity);
            Assert.NotNull(model.Identity);
            Assert.False(model.IsAnonymous);

            TestClaims(user, model, phone, phoneVerified);

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

            TestClaims(user, model, null, null);

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

            TestClaims(user, model, null, null);

            TestSecurityRoles[] nullTest = null;

            Assert.False(model.HasRole(nullTest));

            Assert.True(model.HasRole(TestSecurityRoles.RoleOne));
            Assert.False(model.HasRole(TestSecurityRoles.RoleTwo));
            Assert.True(model.HasRole(TestSecurityRoles.RoleThree));

            Assert.False(model.HasRole(TestSecurityPermissions.PermOne));
            Assert.True(model.HasRole(TestSecurityPermissions.PermTwo));

            Assert.True(model.HasRole(TestSecurityRoles.RoleOne, TestSecurityRoles.RoleThree));
            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleThree));
            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleOne));
            Assert.True(model.HasRole(TestSecurityRoles.RoleTwo, TestSecurityRoles.RoleThree, TestSecurityRoles.RoleOne));
        }


    private void TestClaims(OltAuthenticatedUserJson<OltPersonName> user, TestIdentity model, string phone, bool? phoneVerified)
    {
            var lastName = string.IsNullOrWhiteSpace(user.Name.Suffix) ? user.Name.Last : $"{user.Name.Last} {user.Name.Suffix}";

            Assert.Equal(user.Name.First, model.FirstName);
            Assert.Equal(user.Name.Middle, model.MiddleName);
            Assert.Equal(lastName, model.LastName);
            Assert.Equal(user.FullName, model.FullName);

            Assert.Equal(user.Username, model.Username);
            Assert.Equal(user.Email, model.Email);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal(user.UserPrincipalName, model.UserPrincipalName);
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.Equal(user.NameId, model.NameId);

            if (!string.IsNullOrWhiteSpace(phone))
            {
                Assert.Equal(phone, model.Phone);                
            }

            Assert.Equal(phoneVerified, model.PhoneVerified);
        }

}