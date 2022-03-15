using Microsoft.EntityFrameworkCore;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.SqlServer.Tests.Assests.Entites
{

    public class UserEntity : OltEntityId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NameSuffix { get; set; }

        
        public static UserEntity FakerEntity(bool emptyGuid = false)
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
