using Microsoft.EntityFrameworkCore;
using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.Core.CommandBus.Tests.Assets.EfCore.Entites
{

    public class AddresssEntity : OltEntityId
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class UserEntity : OltEntityId, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

        public string Username { get; set; }

        [Column("EmailAddress")]
        public string Email { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string MiddleName { get; set; }


        [StringLength(100)]
        [Unicode(false)]
        public string LastName { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string NameSuffix { get; set; }


        public static UserEntity FakerEntity(bool emptyGuid = false)
        {
            return new UserEntity
            {
                Id = Faker.RandomNumber.Next(1000, 50000),
                Username = Faker.Internet.UserName(),
                Email = Faker.Internet.Email(),
                FirstName = Faker.Name.First(),
                MiddleName = Faker.Name.Middle(),
                LastName = Faker.Name.Last()
            };
        }
    }

}
