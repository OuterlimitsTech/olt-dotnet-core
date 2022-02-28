using OLT.Core;

namespace OLT.EF.Common.Tests.Assets.Models
{
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
}
