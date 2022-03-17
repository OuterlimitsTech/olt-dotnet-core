using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.Services.Tests.Assets.Entites
{
    public class UserEntity : OltEntityId, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string MiddleName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string NameSuffix { get; set; }
        

        public static UserEntity FakerEntity()
        {
            return new UserEntity
            {
                FirstName = Faker.Name.First(),
                MiddleName = Faker.Name.Middle(),
                LastName = Faker.Name.Last()
            };
        }


    }
}