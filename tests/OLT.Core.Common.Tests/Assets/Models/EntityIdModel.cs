using OLT.Core;
using OLT.Core.Common.Tests.Assets;

namespace OLT.Core.Services.Tests.Assets.Models;

public class EntityIdModel : OltEntityId
{
    public static EntityIdModel FakerData()
    {
        return new EntityIdModel
        {
            Id = Faker.RandomNumber.Next(1000, 10000),
            CreateUser = Faker.Internet.UserName(),
            CreateDate = TestHelper.FakerDateTimePast(),
            ModifyUser = Faker.Internet.UserName(),
            ModifyDate = TestHelper.FakerDateTimePast(),
        };
    }
}