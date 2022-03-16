using OLT.Core;
using System;

namespace OLT.EF.Core.Services.Tests.Assets.Models
{
    public class PersonDto : OltPersonName
    {
        public Guid? UniqueId { get; set; }
        public int? PersonId { get; set; }

        public static PersonDto FakerEntity()
        {
            return new PersonDto
            {
                UniqueId = Guid.NewGuid(),
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
            };
        }
    }
}
