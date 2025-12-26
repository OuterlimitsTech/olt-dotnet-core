using AwesomeAssertions;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltAuthenticatedUserJwtTokenJsonTests
{
    [Fact]
    public void OltAuthenticatedUserJwtTokenJson_Should_Set_And_Get_Properties()
    {
        // Arrange
        var issued = DateTimeOffset.Now;
        var expires = issued.AddHours(1);
        var jwtTokenJson = new OltAuthenticatedUserJwtTokenJson<TestNameModel>
        {
            Token = "testToken",
            Issued = issued,
            Expires = expires,
            NameId = "testNameId",
            Username = "testUsername",
            Email = "testEmail",
            //FullName = "testFullName",
            Name = new TestNameModel 
            { 
                First = "Test",
                Last = "Last"
            },
            TokenType = "Bearer",
            Roles = new List<string> { "Admin" },
            Permissions = new List<string> { "Read", "Write" }
        };

        // Act & Assert
        Assert.Equal("testToken", jwtTokenJson.Token);
        Assert.Equal(issued, jwtTokenJson.Issued);
        Assert.Equal(expires, jwtTokenJson.Expires);
        Assert.Equal((expires - issued).TotalSeconds, jwtTokenJson.ExpiresIn);
        Assert.Equal("testNameId", jwtTokenJson.NameId);
        Assert.Equal("testUsername", jwtTokenJson.Username);
        Assert.Equal("testEmail", jwtTokenJson.Email);
        Assert.Equal("Test Last", jwtTokenJson.FullName);
        Assert.Equal("Test Last", jwtTokenJson.Name.FullName);
        Assert.Equal("Bearer", jwtTokenJson.TokenType);
        Assert.Single(jwtTokenJson.Roles);
        Assert.Equal("Admin", jwtTokenJson.Roles[0]);
        Assert.Equal(2, jwtTokenJson.Permissions.Count);
        Assert.Contains("Read", jwtTokenJson.Permissions);
        Assert.Contains("Write", jwtTokenJson.Permissions);
    }

    [Fact]
    public void OltAuthenticatedUserJwtTokenJson_Should_Be_Empty_By_Default()
    {
        var model = new OltAuthenticatedUserJwtTokenJson<OltPersonName>();
        Assert.NotNull(model.Name);
        Assert.NotNull(model.Name as IOltPersonName);
        Assert.NotNull(model as OltAuthenticatedUserJson<OltPersonName>);

        Assert.Null(model.Username);
        Assert.Null(model.Email);
        Assert.Equal("", model.FullName);
        Assert.Null(model.Token);
        Assert.Null(model.Issued);
        Assert.Null(model.Expires);
        Assert.Null(model.ExpiresIn);
        Assert.Empty(model.Roles);
        Assert.Empty(model.Permissions);
    }

    [Fact]
    public void OltAuthenticatedUserJwtTokenJson_Should_Set_And_Get_Properties_With_Faker()
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
        model.Username = userName;
        model.Email = emailAddress;
        model.Name = name;
        model.Token = token;
        model.Issued = issued;
        model.Expires = expires;
        model.Roles = roles;
        model.Permissions = permissions;

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


    public class TestNameModel : OltPersonName
    {
        
    }
}
