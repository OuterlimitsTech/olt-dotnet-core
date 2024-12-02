using OLT.Core;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.Services.Tests.Assets.Entites
{
    public class PersonEntity : OltEntityIdDeletable, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

        [MaxLength(50)]
        [Required]
        public string NameFirst { get; set; } = default!;

        [MaxLength(50)]
        public string? NameMiddle { get; set; }

        [MaxLength(50)]
        [Required]
        public string NameLast { get; set; } = default!;


        public List<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
        

        public static PersonEntity FakerEntity()
        {
            return new PersonEntity
            {
                UniqueId = Guid.NewGuid(),
                NameFirst = Faker.Name.First(),
                NameMiddle = Faker.Name.Middle(),
                NameLast = Faker.Name.Last(),
            };
        }

        public static List<PersonEntity> FakerList(int number)
        {
            var list = new List<PersonEntity>();

            for (int i = 0; i < number; i++)
            {
                list.Add(FakerEntity());
            }

            return list;
        }
    }
}