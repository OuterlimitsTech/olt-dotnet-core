namespace OLT.Core.Model.Abstractions.Tests;

public static class TestHelper
{
    public static OltPersonName FakerPersonName(string? nameSuffix)
    {
        return new OltPersonName
        {
            First = Faker.Name.First(),
            Middle = Faker.Name.Middle(),
            Last = Faker.Name.Last(),
            Suffix = string.IsNullOrWhiteSpace(nameSuffix) ? null : nameSuffix,
        };
    }

    public static OltAuthenticatedUserJson<OltPersonName> FakerAuthUser(string nameSuffix)
    {
        return new OltAuthenticatedUserJson<OltPersonName>()
        {
            Name = FakerPersonName(nameSuffix),
            NameId = Faker.RandomNumber.Next(1050).ToString(),
            Username = Faker.Internet.UserName(),
            Email = Faker.Internet.Email(),
            TokenType = Faker.Internet.DomainWord(),
            Roles = FakerRoleList("role-", 8, 15),
            Permissions = FakerRoleList("perm-", 10, 23)
        };
    }

    public static OltAuthenticatedUserJwtTokenJson<OltPersonName> FakerAuthUserToken(string nameSuffix)
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

    public static List<string> FakerRoleList(string prefix, int minMix = 5, int maxMix = 10)
    {
        var list = new List<string>();
        for (int i = 1; i <= Faker.RandomNumber.Next(minMix, maxMix); i++)
        {
            list.Add($"{prefix}{i}");
        }
        return list;
    }

}