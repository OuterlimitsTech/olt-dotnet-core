namespace OLT.Core.Common.Tests.Assets.Models;

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