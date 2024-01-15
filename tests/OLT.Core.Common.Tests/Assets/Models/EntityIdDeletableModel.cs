namespace OLT.Core.Common.Tests.Assets.Models;

public class EntityIdDeletableModel : OltEntityIdDeletable
{
    public static EntityIdDeletableModel FakerData()
    {
        return new EntityIdDeletableModel
        {
            Id = Faker.RandomNumber.Next(1000, 10000),
            DeletedBy = Faker.Internet.UserName(),
            DeletedOn = TestHelper.FakerDateTimePast(),
            CreateUser = Faker.Internet.UserName(),
            CreateDate = TestHelper.FakerDateTimePast(),
            ModifyUser = Faker.Internet.UserName(),
            ModifyDate = TestHelper.FakerDateTimePast(),
        };
    }
}