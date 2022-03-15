using OLT.Core;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public NameAutoMapperModel Name { get; set; } = new NameAutoMapperModel();
    }

    public class UserDto : OltPersonName
    {
        public int? UserId { get; set; }        
    }
}
