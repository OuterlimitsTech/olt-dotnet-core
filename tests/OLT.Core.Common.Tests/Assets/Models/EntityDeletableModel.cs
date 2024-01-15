using OLT.Core;
using OLT.Core.Common.Tests.Assets;

namespace OLT.Core.Services.Tests.Assets.Models;

public class EntityDeletableModel : OltEntityDeletable
{

    public static EntityDeletableModel FakerData()
    {
            return new EntityDeletableModel
            {
                DeletedBy = Faker.Internet.UserName(),
                DeletedOn = TestHelper.FakerDateTimePast(),
                CreateUser = Faker.Internet.UserName(),
                CreateDate = TestHelper.FakerDateTimePast(),
                ModifyUser = Faker.Internet.UserName(),
                ModifyDate = TestHelper.FakerDateTimePast(),
            };
        }
}