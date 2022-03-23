using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Tests.Assets
{
    public class PersonDto : OltPersonName
    {
        public Guid? UniqueId { get; set; }
        public int? PersonId { get; set; }
        public string Email { get; set; }

        public static PersonDto FakerData()
        {
            return new PersonDto
            {
                First = Faker.Name.First(),
                Middle = Faker.Name.Middle(),
                Last = Faker.Name.Last(),
                Email = Faker.Internet.Email()
            };
        }
    }
}
