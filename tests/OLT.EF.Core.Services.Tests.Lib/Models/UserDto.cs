using OLT.Core;
using System;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class UserDto : OltPersonName
    {
        public int? UserId { get; set; }
        public Guid UserGuid { get; set; }

        public static UserDto FakerEntity()
        {
            return new UserDto
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
            };
        }
    }
}
