namespace OLT.Core.Model.Abstractions.Tests;

public static class TestHelper
{
    public class TestPersonNameModel
    {
        public string? NameFirst { get; set; }
        public string? NameLast { get; set; }

        public static List<TestPersonNameModel> FakerList(int number)
        {
            var list = new List<TestPersonNameModel>();

            for (int i = 0; i < number; i++)
            {
                list.Add(new TestPersonNameModel
                {
                    NameFirst = Faker.Name.First(),
                    NameLast = Faker.Name.Last(),
                });
            }

            return list;
        }

    }

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


    public static DateTimeOffset FakerDateTimePast()
    {
        var milliseconds = Faker.RandomNumber.Next(10000, 100000) * -1;
        return DateTimeOffset.Now.AddMilliseconds(milliseconds);
    }


    /// <summary>
    /// Randomize the List
    /// </summary>
    /// <param name="expected">Expected list of data to randomly add to results</param>
    /// <param name="startingId"></param>
    /// <param name="minMix"></param>
    /// <param name="maxMix"></param>
    /// <returns></returns>
    public static List<EntityPersonModel> BuildTestList(List<EntityPersonModel> expected, int startingId = 1000, int minMix = 0, int maxMix = 6)
    {
        var randomized = expected.OrderBy(x => Guid.NewGuid()).ToList();
        var list = new List<EntityPersonModel>();
        for (int i = 0; i < randomized.Count; i++)
        {
            var rangeList = EntityPersonModel.FakerList(Faker.RandomNumber.Next(minMix, maxMix));
            rangeList.ForEach(item =>
            {
                item.Id = startingId;
                startingId++;
                list.Add(item);
            });

            randomized[i].Id = startingId;
            startingId++;
            list.Add(randomized[i]);
        }
        return list;
    }

    public static List<EntityPersonModel> BuildTestList(EntityPersonModel expected, int startingId = 1000, int minMix = 4, int maxMix = 8)
    {
        return BuildTestList(new List<EntityPersonModel> { expected }, startingId, minMix, maxMix);
    }

    public static void SetFirstName(this List<EntityPersonModel> list, string firstName)
    {
        list.ForEach(person => person.SetFirstName(firstName));
    }

    public static void SetFirstName(this EntityPersonModel person, string firstName)
    {
        person.FirstName = firstName;
    }

    public static void SetFirstNameStartsWith(this List<EntityPersonModel> list, string firstName)
    {
        list.ForEach(person => person.SetFirstNameStartsWith(firstName));
    }

    public static void SetFirstNameStartsWith(this EntityPersonModel person, string firstName)
    {
        person.FirstName = $"{firstName}{person.FirstName}";
    }

    public static void SetFirstNameEndsWith(this List<EntityPersonModel> list, string firstName)
    {
        list.ForEach(person => person.SetFirstNameEndsWith(firstName));
    }

    public static void SetFirstNameEndsWith(this EntityPersonModel person, string firstName)
    {
        person.FirstName = $"{person.FirstName}{firstName}";
    }

    public static void SetFirstNameContains(this List<EntityPersonModel> list, string firstName)
    {
        list.ForEach(person => person.SetFirstNameContains(firstName));
    }

    public static void SetFirstNameContains(this EntityPersonModel person, string firstName)
    {
        person.FirstName = $"{person.FirstName}{firstName}{Faker.Name.Last()}";
    }



    public static void SetLastNameStartsWith(this List<EntityPersonModel> list, string lastName)
    {
        list.ForEach(person => person.SetLastNameStartsWith(lastName));
    }

    public static void SetLastNameStartsWith(this EntityPersonModel person, string lastName)
    {
        person.LastName = $"{lastName}{person.LastName}";
    }

}