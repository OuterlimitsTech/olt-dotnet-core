using OLT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.EF.Core.Services.Tests.Assets.Entites
{
    public class PersonEntity : OltEntityIdDeletable
    {
        [MaxLength(50)]
        [Required]
        public string NameFirst { get; set; }

        [MaxLength(50)]
        public string NameMiddle { get; set; }

        [MaxLength(50)]
        [Required]
        public string NameLast { get; set; }


        public List<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();

        public static PersonEntity FakerEntity()
        {
            return new PersonEntity
            {
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