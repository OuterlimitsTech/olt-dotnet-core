using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations
{


    public class UserEntityTestConfiguration : OltEntityTypeConfiguration<UserEntity>
    {

        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(c => c.FirstName).IsRequired();


            var list = new List<UserEntity>();
            list.Add(UserEntity.FakerEntity());
            list.Add(UserEntity.FakerEntity());
            list.Add(UserEntity.FakerEntity());            
        }
    }
}
