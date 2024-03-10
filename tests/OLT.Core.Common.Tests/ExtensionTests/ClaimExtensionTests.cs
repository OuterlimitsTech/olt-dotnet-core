using FluentAssertions;
using OLT.Constants;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests.ExtensionTests;

public class ClaimExtensionTests
{
    const int nameClaims = 3;
    const int userClaims = 7;

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
        Assert.Contains(claims, p => p.Type == OltClaimTypes.GivenName);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.MiddleName);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.FamilyName);

        Assert.Contains(claims, p => p.Type == OltClaimTypes.GivenName && p.Value == model.First);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.MiddleName && p.Value == model.Middle);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.FamilyName && p.Value == lastName);
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
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Name);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Email);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.TokenType);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.NameId);            
        Assert.Contains(claims, p => p.Type == OltClaimTypes.PreferredUsername);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Username);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Nickname);

        Assert.Contains(claims, p => p.Type == OltClaimTypes.Name && p.Value == model.FullName);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Email && p.Value == model.Email);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.TokenType && p.Value == model.TokenType);
#pragma warning disable CS0618 // Type or member is obsolete
        Assert.Contains(claims, p => p.Type == OltClaimTypes.TokenType && p.Value == model.AuthenticationType);  //Legacy Check
#pragma warning restore CS0618 // Type or member is obsolete
        Assert.Contains(claims, p => p.Type == OltClaimTypes.NameId && p.Value == model.NameId);
#pragma warning disable CS0618 // Type or member is obsolete
        Assert.Contains(claims, p => p.Type == OltClaimTypes.NameId && p.Value == model.UserPrincipalName);  //Legacy Check
#pragma warning restore CS0618 // Type or member is obsolete
        Assert.Contains(claims, p => p.Type == OltClaimTypes.PreferredUsername && p.Value == model.Username);
        Assert.Contains(claims, p => p.Type == OltClaimTypes.Nickname && p.Value == model.Name.First);

        var roleClaims = new List<string>();
        roleClaims.AddRange(model.Roles);
        roleClaims.AddRange(model.Permissions);

        claims.Where(p => p.Type == OltClaimTypes.Role).Select(s => s.Value).Should().BeEquivalentTo(roleClaims);
    }

    [Theory]
    [InlineData(0, "Claim", null)]
    [InlineData(1, "Claim", "Value")]
    [InlineData(0, "Claim", "")]
    [InlineData(0, "Claim", " ")]
    public void AddClaim(int expectedCount, string type, string value)
    {
        var list = new List<System.Security.Claims.Claim>();
        list.AddClaim(type, value);
        list.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(0, "Claim", "")]
    [InlineData(0, "Claim", " ")]
    [InlineData(1, "Claim", "Value")]
    public void AddClaimWithClaimObject(int expectedCount, string type, string value)
    {
        var list = new List<System.Security.Claims.Claim>();
        list.AddClaim(new System.Security.Claims.Claim(type, value));
        list.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void AddClaimException()
    {
        Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, null));
        Assert.Throws<ArgumentNullException>(() => new List<System.Security.Claims.Claim>().AddClaim(null));
        Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, new System.Security.Claims.Claim("Claim", "Value")));


        Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, null, null));
        Assert.Throws<ArgumentNullException>(() => new List<System.Security.Claims.Claim>().AddClaim(null, null));
        Assert.Throws<ArgumentNullException>(() => OltClaimExtensions.AddClaim(null, "Claim", "Value"));

        try
        {
            new List<System.Security.Claims.Claim>().AddClaim("Claim", null);
            Assert.True(true);
        }
        catch
        {
            Assert.False(true);
        }
    }
}