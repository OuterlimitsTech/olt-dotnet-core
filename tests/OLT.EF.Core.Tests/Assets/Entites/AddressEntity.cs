using Microsoft.EntityFrameworkCore.ChangeTracking;
using OLT.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.EF.Core.Tests.Assets.Entites
{
    [Table("Address")]
    public class AddressEntity : OltEntityId
    {
        [Column("AddressId")]
        public override int Id { get; set; }

        public int PersonId { get; set; }
        public PersonEntity Person { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [StringLength(50)]
        public string City { get; set; }

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
