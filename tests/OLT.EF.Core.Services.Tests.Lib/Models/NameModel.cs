using OLT.Core;

namespace OLT.EF.Core.Services.Tests.Assets.Models;

public class NameModel : OltPersonName
{

    public static NameModel FakerEntity()
    {
        return new NameModel
        {
            First = Faker.Name.First(),
            Middle = Faker.Name.Middle(),
            Last = Faker.Name.Last(),
        };
    }

}
