namespace OLT.EF.Core.Services.Tests.Assets.Models
{

    public class UserModel
    {
        public int? UserId { get; set; }
        public Guid UserGuid { get; set; }
        public NameModel Name { get; set; } = new NameModel();

        public static UserModel FakerEntity()
        {
            return new UserModel
            {
                UserGuid = Guid.NewGuid(),
                Name = NameModel.FakerEntity(),
            };
        }
    }
}
