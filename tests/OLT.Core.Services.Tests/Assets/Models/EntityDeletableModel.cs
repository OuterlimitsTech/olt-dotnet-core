using OLT.Core;

namespace OLT.Core.Services.Tests.Assets.Models
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

    public class EntityMaintainableModel : OltEntityId, IOltEntityMaintainable
    {
        public bool? MaintAdd { get; set; }
        public bool? MaintUpdate { get; set; }
        public bool? MaintDelete { get; set; }

        public static EntityMaintainableModel FakerData()
        {
            return new EntityMaintainableModel
            {                
                CreateUser = Faker.Internet.UserName(),
                CreateDate = TestHelper.FakerDateTimePast(),
                ModifyUser = Faker.Internet.UserName(),
                ModifyDate = TestHelper.FakerDateTimePast(),
            };
        }
    }
}
