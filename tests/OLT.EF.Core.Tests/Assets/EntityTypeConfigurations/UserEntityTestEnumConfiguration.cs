using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations
{
    public enum UserEntityTypes
    {
        [Code("TestAccount", 1500)]
        [KeyValue("Email", "test@test.com")]
        [KeyValue("FirstName", "Test")]
        [KeyValue("LastName", "Account")]
        [Description("Test Account")]
        [UniqueId("0bdd9387-0348-441a-bc2d-49f043689922")]
        TestAccount = 500,

        [Code("SystemLoad")]
        [KeyValue("Email", "systemload@test.com")]
        [KeyValue("FirstName", "System")]
        [KeyValue("LastName", "Load")]
        [Description("System Load")]
        [UniqueId("ba1fa666-8ea9-4b8c-91f9-8c7589b98894")]
        SystemLoad = 530,
    }

    public class UserEntityTestEnumConfiguration : OltEntityTypeConfiguration<UserEntity, UserEntityTypes>
    {

        public List<UserEntity> Results { get; } = new List<UserEntity>();

        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            Assert.Equal("EmailAddress", base.GetColumnName(builder.Property(p => p.Email).Metadata.PropertyInfo));
            Assert.Null(base.GetColumnName(builder.Property(p => p.FirstName).Metadata.PropertyInfo));
            
            builder.Property(c => c.LastName).IsRequired();
            base.Configure(builder);

        }


        protected override void Map(UserEntity entity, UserEntityTypes @enum)
        {
            base.Map(entity, @enum);

            var keyValue = @enum.GetKeyValueAttributes();

            var fullName = @enum.GetDescription();
            var sortOrder = @enum.GetCodeEnumSort();            
            var uid = OltAttributeExtensions.GetAttributeInstance<UniqueIdAttribute, UserEntityTypes>(@enum)?.UniqueId;
            

            entity.Username = GetEnumCode(@enum);
            entity.Email = keyValue.First(p => p.Key == "Email").Value;
            entity.FirstName = keyValue.First(p => p.Key == "FirstName")?.Value;
            entity.LastName = keyValue.First(p => p.Key == "LastName")?.Value;
            
            Results.Add(entity);

            Assert.Equal(fullName, base.GetEnumDescription(@enum));
            Assert.Equal(entity.Username, base.GetEnumCode(@enum));
            Assert.Equal(sortOrder, base.GetEnumCodeSortOrder(@enum));
            Assert.Equal(uid, base.GetUniqueId(@enum));

        }

    }
}
