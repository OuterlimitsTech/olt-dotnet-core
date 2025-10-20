using AwesomeAssertions;
using OLT.Identity.Abstractions;
using System.Security.Claims;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltClaimExtensionsTests
{
    [Fact]
    public void ToClaims_ShouldReturnCorrectClaims()
    {
        var fullName = "Test Middle User Jr";

        // Arrange
        var user = new OltAuthenticatedUserJson<OltPersonName>
        {
            NameId = "123",
            Username = "testuser",
            Email = "testuser@example.com",
            TokenType = "Bearer",
            Name = new OltPersonName
            {
                First = "Test",
                Middle = "Middle",
                Last = "User",
                Suffix = "Jr"
            },
            Roles = new List<string> { "Admin", "User" },
            Permissions = new List<string> { "Read", "Write" }
        };

        // Act
        var claims = user.ToClaims();
        
        Assert.Equal(fullName, user.FullName);

        // Assert
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Name && c.Value == fullName);
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Email && c.Value == "testuser@example.com");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.TokenType && c.Value == "Bearer");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.NameId && c.Value == "123");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.PreferredUsername && c.Value == "testuser");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Username && c.Value == "testuser");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Nickname && c.Value == "Test");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.GivenName && c.Value == "Test");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.MiddleName && c.Value == "Middle");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.FamilyName && c.Value == "User Jr");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Role && c.Value == "Admin");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Role && c.Value == "User");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Role && c.Value == "Read");
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Role && c.Value == "Write");



        OltAuthenticatedUserJson<OltPersonName> model = TestHelper.FakerAuthUser(Faker.Name.Suffix());
        model.Roles.AddRange(TestHelper.FakerRoleList("role-", 5, 9));
        model.Permissions.AddRange(TestHelper.FakerRoleList("perm-", 6, 12));
        claims = model.ToClaims();

        var totalClaims = nameClaims + userClaims + model.Roles.Count + model.Permissions.Count;

        Assert.Equal(totalClaims, claims.Count);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Name);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Email);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.TokenType);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.NameId);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.PreferredUsername);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Username);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Nickname);

        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Name && p.Value == model.FullName);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Email && p.Value == model.Email);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.TokenType && p.Value == model.TokenType);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.NameId && p.Value == model.NameId);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.PreferredUsername && p.Value == model.Username);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.Nickname && p.Value == model.Name.First);

        var roleClaims = new List<string>();
        roleClaims.AddRange(model.Roles);
        roleClaims.AddRange(model.Permissions);

        claims.Where(p => p.Type == ClaimTypeNames.Role).Select(s => s.Value).Should().BeEquivalentTo(roleClaims);
    }

    [Fact]
    public void ToClaims_ShouldThrowArgumentNullException_WhenModelIsNull()
    {
        // Arrange
        OltAuthenticatedUserJson<OltPersonName>? user = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => user.ToClaims());
    }

    [Fact]
    public void AddClaim_ShouldAddClaim_WhenValueIsNotNullOrEmpty()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act
        claims.AddClaim(ClaimTypeNames.Name, "Test User");

        // Assert
        Assert.Contains(claims, c => c.Type == ClaimTypeNames.Name && c.Value == "Test User");
    }

    [Fact]
    public void AddClaim_ShouldNotAddClaim_WhenValueIsNullOrEmpty()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act
        claims.AddClaim(ClaimTypeNames.Name, null);

        // Assert
        Assert.DoesNotContain(claims, c => c.Type == ClaimTypeNames.Name);
    }

    [Fact]
    public void AddClaim_ShouldThrowArgumentNullException_WhenClaimsIsNull()
    {
        // Arrange
        List<Claim>? claims = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => claims.AddClaim(ClaimTypeNames.Name, "Test User"));
    }

    [Fact]
    public void AddClaim_ShouldThrowArgumentNullException_WhenTypeIsNull()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => claims.AddClaim(null, "Test User"));
    }

    const int nameClaims = 3;
    const int userClaims = 7;


    [Theory]
    [InlineData(null)]
    [InlineData("Jr.")]
    public void ToClaimsOltPerson_ShouldReturnCorrectClaims(string? suffix)
    {
        IOltPersonName? model = null;
        Assert.Throws<ArgumentNullException>(() => model.ToClaims());

        model = TestHelper.FakerPersonName(suffix);
        var claims = model.ToClaims();
        var lastName = string.IsNullOrWhiteSpace(model.Suffix) ? model.Last : $"{model.Last} {model.Suffix}";

        Assert.Equal(nameClaims, claims.Count);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.GivenName);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.MiddleName);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.FamilyName);

        Assert.Contains(claims, p => p.Type == ClaimTypeNames.GivenName && p.Value == model.First);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.MiddleName && p.Value == model.Middle);
        Assert.Contains(claims, p => p.Type == ClaimTypeNames.FamilyName && p.Value == lastName);
    }

    [Theory]
    [InlineData(0, "Claim", null)]
    [InlineData(1, "Claim", "Value")]
    [InlineData(0, "Claim", "")]
    [InlineData(0, "Claim", " ")]
    public void AddClaim_ShouldAddClaimBasedOnValue(int expectedCount, string type, string? value)
    {
        var list = new List<System.Security.Claims.Claim>();
        list.AddClaim(type, value);
        list.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(0, "Claim", "")]
    [InlineData(0, "Claim", " ")]
    [InlineData(1, "Claim", "Value")]
    public void AddClaimWithClaimObject_ShouldAddClaimBasedOnValue(int expectedCount, string type, string value)
    {
        var list = new List<System.Security.Claims.Claim>();
        list.AddClaim(new System.Security.Claims.Claim(type, value));
        list.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void AddClaimException_ShouldThrowArgumentNullException()
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
