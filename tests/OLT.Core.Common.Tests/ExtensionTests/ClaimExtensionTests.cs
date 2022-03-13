using FluentAssertions;
using OLT.Constants;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Common.Tests.ExtensionTests
{
    public class ClaimExtensionTests
    {
        const int nameClaims = 3;
        const int userClaims = 5;

        [Theory]
        [InlineData(null)]
        [InlineData("Jr.")]
        public void ToClaimsOltPerson(string suffix)
        {
            IOltPersonName model = null;
            Assert.Throws<ArgumentNullException>(() => model.ToClaims());


            model = TestHelper.FakerPersonName(suffix);
            var claims = model.ToClaims();
            var lastName = string.IsNullOrWhiteSpace(model.Suffix) ? model.Last : $"{model.Last} {model.Suffix}";

            Assert.Equal(nameClaims, claims.Count);
            Assert.Contains(claims, p => p.Type == ClaimTypes.GivenName);
            Assert.Contains(claims, p => p.Type == OltClaimTypes.MiddleName);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Surname);

            Assert.Contains(claims, p => p.Type == ClaimTypes.GivenName && p.Value == model.First);
            Assert.Contains(claims, p => p.Type == OltClaimTypes.MiddleName && p.Value == model.Middle);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Surname && p.Value == lastName);
        }


        [Fact]
        public void ToClaimsOltAuthenticatedUserJson()
        {
            OltAuthenticatedUserJson<OltPersonName> model = null;
            Assert.Throws<ArgumentNullException>(() => model.ToClaims());


            model = TestHelper.FakerAuthUser(Faker.Name.Suffix());
            model.Roles.AddRange(TestHelper.FakerRoleList("role-", 5, 9));
            model.Permissions.AddRange(TestHelper.FakerRoleList("perm-", 6, 12));
            var claims = model.ToClaims();

            var totalClaims = nameClaims + userClaims + model.Roles.Count + model.Permissions.Count;

            Assert.Equal(totalClaims, claims.Count);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Name);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Email);
            Assert.Contains(claims, p => p.Type == ClaimTypes.AuthenticationMethod);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Upn);
            Assert.Contains(claims, p => p.Type == ClaimTypes.NameIdentifier);

            Assert.Contains(claims, p => p.Type == ClaimTypes.Name && p.Value == model.FullName);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Email && p.Value == model.Email);
            Assert.Contains(claims, p => p.Type == ClaimTypes.AuthenticationMethod && p.Value == model.AuthenticationType);
            Assert.Contains(claims, p => p.Type == ClaimTypes.Upn && p.Value == model.UserPrincipalName);
            Assert.Contains(claims, p => p.Type == ClaimTypes.NameIdentifier && p.Value == model.Username);

            var roleClaims = new List<string>();
            roleClaims.AddRange(model.Roles);
            roleClaims.AddRange(model.Permissions);

            claims.Where(p => p.Type == ClaimTypes.Role).Select(s => s.Value).Should().BeEquivalentTo(roleClaims);
        }

        [Theory]
        [InlineData(0, "Claim", null)]
        [InlineData(1, "Claim", "Value")]
        [InlineData(0, "Claim", "")]
        [InlineData(0, "Claim", " ")]
        public void AddClaim(int expectedCount, string type, string value)
        {
            var list = new List<Claim>();
            list.AddClaim(type, value);
            list.Should().HaveCount(expectedCount);
        }

        [Theory]
        [InlineData(0, "Claim", "")]
        [InlineData(0, "Claim", " ")]
        [InlineData(1, "Claim", "Value")]
        public void AddClaimWithClaimObject(int expectedCount, string type, string value)
        {
            var list = new List<Claim>();
            list.AddClaim(new Claim(type, value));
            list.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void AddClaimException()
        {
            Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, null));
            Assert.Throws<ArgumentNullException>(() => new List<Claim>().AddClaim(null));
            Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, new Claim("Claim", "Value")));


            Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new List<Claim>().AddClaim(null, null));
            Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, "Claim", "Value"));

            try
            {
                new List<Claim>().AddClaim("Claim", null);
                Assert.True(true);
            }
            catch
            {
                Assert.False(true);
            }
        }
    }
}
