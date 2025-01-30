using OLT.Identity.Abstractions;
using System.Security.Claims;
using System.Security.Principal;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltIdentityTests
{

    [Fact]
    public void IsAnonymous_ShouldReturnTrue_WhenIdentityIsNull()
    {
        // Arrange
        var identity = new TestIdentity();

        // Act
        var result = identity.IsAnonymous;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAnonymous_ShouldReturnTrue_WhenUsernameIsNull()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var result = oltIdentity.IsAnonymous;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAnonymous_ShouldReturnFalse_WhenUsernameIsNotNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Username, "testuser")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var result = oltIdentity.IsAnonymous;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetClaims_ShouldReturnCorrectClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Username, "testuser"),
            new Claim(ClaimTypeNames.Email, "testuser@example.com")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var usernameClaims = oltIdentity.GetClaims(ClaimTypeNames.Username);
        var emailClaims = oltIdentity.GetClaims(ClaimTypeNames.Email);

        // Assert
        Assert.Single(usernameClaims);
        Assert.Equal("testuser", usernameClaims.First().Value);
        Assert.Single(emailClaims);
        Assert.Equal("testuser@example.com", emailClaims.First().Value);
    }

    [Fact]
    public void GetDbUsername_ShouldReturnUsername_WhenUsernameIsNotNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Username, "testuser")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var dbUsername = oltIdentity.GetDbUsername();

        // Assert
        Assert.Equal("testuser", dbUsername);
    }

    [Fact]
    public void GetDbUsername_ShouldReturnFullName_WhenUsernameIsNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Name, "Test User")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var dbUsername = oltIdentity.GetDbUsername();

        // Assert
        Assert.Equal("Test User", dbUsername);
    }

    [Fact]
    public void HasRole_ShouldReturnTrue_WhenRoleExists()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Role, "Admin")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var oltIdentity = new TestIdentity(principal);

        // Act
        var hasRole = oltIdentity.HasRole("Admin");

        // Assert
        Assert.True(hasRole);


        var user = OltIdentityTests.FakerAuthUserToken(Faker.Name.Suffix());
        user.Roles.Add(TestSecurityRoles.RoleOne.GetCodeEnumSafe());
        user.Roles.Add(TestSecurityRoles.RoleThree.GetCodeEnumSafe());
        user.Permissions.Add(TestSecurityPermissions.PermTwo.GetCodeEnumSafe());
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
    public void HasRole_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypeNames.Role, "User")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var oltIdentity = new TestIdentity(principal);

        // Act
        var hasRole = oltIdentity.HasRole("Admin");

        // Assert
        Assert.False(hasRole);
    }

    [Fact]
    public void IdentityTestEmpty()
    {
        var model = new TestIdentity();
        Assert.NotNull(model as IOltIdentity);
        Assert.Null(model.Identity);
        Assert.True(model.IsAnonymous);
        Assert.Empty(model.GetAllClaims());
        Assert.Empty(model.GetClaims(ClaimTypeNames.Name));
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
        Assert.Null(model.NameId);
        Assert.Null(model.Subject);
        Assert.Null(model.PhoneVerified);
    }

    [Fact]
    public void IsAnonymous_ShouldReturnTrue_WhenIdentityIsEmpty()
    {
        var model = new TestIdentity();
        Assert.True(model.IsAnonymous);

        var user = OltIdentityTests.FakerAuthUserToken(Faker.Name.Suffix());
        var identity = new GenericIdentity(user.FullName);
        model = new TestIdentity(identity);
        Assert.True(model.IsAnonymous);

        identity.AddClaim(new System.Security.Claims.Claim(ClaimTypeNames.PreferredUsername, Faker.Internet.UserName()));
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
    public void Claims_ShouldReturnCorrectClaims(string? nameSuffix, string? phone, bool? phoneVerified)
    {
        var user = OltIdentityTests.FakerAuthUserToken(nameSuffix);

        var claims = user.ToClaims();
        claims.AddClaim(ClaimTypeNames.PhoneNumber, phone);
        claims.AddClaim(ClaimTypeNames.PhoneNumberVerified, phoneVerified?.ToString());

        var identity = new GenericIdentity(user.FullName);
        identity.AddClaims(claims);

        var model = new TestIdentity(identity);
        Assert.NotNull(model.Identity);
        Assert.False(model.IsAnonymous);

        TestClaims(user, model, phone, phoneVerified);

        Assert.Equal(claims.Count + 1, model.GetAllClaims().Count);  //GenericIdentity adds the ClaimTypes.Name
    }

    //[Fact]
    //public void HasRole_ShouldReturnTrue_WhenRoleExists()
    //{
    //    var user = TestHelper.FakerAuthUserToken(Faker.Name.Suffix());
    //    user.Roles.Add(TestSecurityRoles.RoleOne.GetCodeEnumSafe());
    //    user.Roles.Add(TestSecurityRoles.RoleThree.GetCodeEnumSafe());
    //    user.Permissions.Add(TestSecurityPermissions.PermTwo.GetCodeEnumSafe());
    //    var identity = new GenericIdentity(user.FullName);
    //    identity.AddClaims(user.ToClaims());

    //    var model = new TestIdentity(identity);

    //    user.Roles.ForEach(role => Assert.True(model.HasRole(role)));
    //    user.Permissions.ForEach(perm => Assert.True(model.HasRole(perm)));

    //    TestClaims(user, model, null, null);

    //    Assert.False(model.HasRole(TestSecurityRoles.RoleTwo.GetCodeEnum()));
    //    Assert.False(model.HasRole(TestSecurityPermissions.PermOne.GetCodeEnum()));
    //    Assert.False(model.HasRole(null));
    //    Assert.False(model.HasRole(""));
    //    Assert.False(model.HasRole(" "));
    //}

    [Fact]
    public void HasRoleEnum_ShouldReturnCorrectRoleStatus()
    {
        var user = OltIdentityTests.FakerAuthUserToken(Faker.Name.Suffix());
        user.Roles.Add(TestSecurityRoles.RoleOne.GetCodeEnumSafe());
        user.Roles.Add(TestSecurityRoles.RoleThree.GetCodeEnumSafe());
        user.Permissions.Add(TestSecurityPermissions.PermTwo.GetCodeEnumSafe());
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


    private void TestClaims(OltAuthenticatedUserJson<OltPersonName> user, TestIdentity model, string? phone, bool? phoneVerified)
    {
        var lastName = string.IsNullOrWhiteSpace(user.Name.Suffix) ? user.Name.Last : $"{user.Name.Last} {user.Name.Suffix}";

        Assert.Equal(user.Name.First, model.FirstName);
        Assert.Equal(user.Name.Middle, model.MiddleName);
        Assert.Equal(lastName, model.LastName);
        Assert.Equal(user.FullName, model.FullName);

        Assert.Equal(user.Username, model.Username);
        Assert.Equal(user.Email, model.Email);
        Assert.Equal(user.NameId, model.NameId);

        if (!string.IsNullOrWhiteSpace(phone))
        {
            Assert.Equal(phone, model.Phone);
        }

        Assert.Equal(phoneVerified, model.PhoneVerified);
    }

    private enum TestSecurityRoles
    {
        [Code("role-one")]
        RoleOne = 1000,

        [Code("role-two")]
        RoleTwo = 2000,

        [Code("role-three")]
        RoleThree = 3000,
    }

    private enum TestSecurityPermissions
    {
        [Code("perm-one")]
        PermOne = 11000,

        [Code("perm-two")]
        PermTwo = 12000,
    }

    private static OltPersonName FakerPersonName(string? nameSuffix)
    {
        return new OltPersonName
        {
            First = Faker.Name.First(),
            Middle = Faker.Name.Middle(),
            Last = Faker.Name.Last(),
            Suffix = string.IsNullOrWhiteSpace(nameSuffix) ? null : nameSuffix,
        };
    }

    private static OltAuthenticatedUserJwtTokenJson<OltPersonName> FakerAuthUserToken(string? nameSuffix)
    {
        return new OltAuthenticatedUserJwtTokenJson<OltPersonName>()
        {
            Name = FakerPersonName(nameSuffix),
            NameId = Faker.RandomNumber.Next(1050).ToString(),
            Username = Faker.Internet.UserName(),
            Email = Faker.Internet.Email(),
            TokenType = Faker.Internet.DomainWord(),
            Token = Faker.Lorem.Words(8).Last(),
            Issued = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(2, 10)),
            Expires = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(20, 40)),
            Roles = FakerRoleList("role-", 8, 15),
            Permissions = FakerRoleList("perm-", 10, 23)
        };
    }
    private static List<string> FakerRoleList(string prefix, int minMix = 5, int maxMix = 10)
    {
        var list = new List<string>();
        for (int i = 1; i <= Faker.RandomNumber.Next(minMix, maxMix); i++)
        {
            list.Add($"{prefix}{i}");
        }
        return list;
    }

}
