using OLT.Core;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.Services.Tests.Assets.Entites
{
    public class AddressEntity : OltEntityId
    {
        public override int Id { get; set; }

        public int PersonId { get; set; }
        public PersonEntity Person { get; set; } = default!;

        [StringLength(50)]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        public static AddressEntity FakerEntity()
        {
            return new AddressEntity
            {
                Street = Faker.Address.StreetAddress(),
                City = Faker.Address.City()
            };
        }

    }

}
