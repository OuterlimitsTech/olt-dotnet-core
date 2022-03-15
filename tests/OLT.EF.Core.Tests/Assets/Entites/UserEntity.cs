using Microsoft.EntityFrameworkCore;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets.Entites
{

    public class UserEntity : OltEntityId, IOltEntityUniqueId
    {
        public Guid UniqueId { get; set; }

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
                FirstName = Faker.Name.First(),
                MiddleName = Faker.Name.Middle(),
                LastName = Faker.Name.Last()
            };
        }
    }

}
