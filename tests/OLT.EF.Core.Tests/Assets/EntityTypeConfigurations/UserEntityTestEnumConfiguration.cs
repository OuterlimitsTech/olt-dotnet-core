using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;
using OLT.Constants;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations
{
    public enum UserEntityTypes
    {
        [Code("TestAccount")]
        [SortOrder(1500)]
        [KeyValue("Email", "test@test.com")]
        [KeyValue("FirstName", "Test")]
        [KeyValue("LastName", "Account")]
        [Description("Test Account")]
        [UniqueId("0bdd9387-0348-441a-bc2d-49f043689922")]
        [EnumMember(Value = "test-account")]
        TestAccount = 500,

        [Code("SystemLoad")]
        [KeyValue("Email", "systemload@test.com")]
        [KeyValue("FirstName", "System")]
        [KeyValue("LastName", "Load")]
        [Description("System Load")]
        [UniqueId("ba1fa666-8ea9-4b8c-91f9-8c7589b98894")]
        //[EnumMember(Value = "system-load")]  
        SystemLoad = 530,


        ErrorAccount = 540,
    }

    public class UserEntityTestEnumConfiguration : OltEntityTypeConfiguration<UserEntity, UserEntityTypes>
    {
        private short testDefaultSort = short.MaxValue - 10;
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

            //if (@enum == UserEntityTypes.ErrorAccount && System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Break();
            //}


            var keyValue = OltKeyValueAttributeExtensions.GetKeyValueAttributes(@enum);
            var fullName = OltAttributeExtensions.GetDescription(@enum);
            var sortOrder = OltSortOrderAttributeExtensions.GetSortOrderEnum(@enum);
            var sortOrderDifferentDefault = OltSortOrderAttributeExtensions.GetSortOrderEnum(@enum, testDefaultSort);
            var uid = OltAttributeExtensions.GetAttributeInstance<UniqueIdAttribute, UserEntityTypes>(@enum)?.UniqueId;
            var enumMember = OltAttributeExtensions.GetAttributeInstance<EnumMemberAttribute, UserEntityTypes>(@enum)?.Value;


            entity.Username = GetEnumCode(@enum);
            
            Assert.Equal(entity.Username, base.GetEnumCode(@enum));
            Assert.Equal(uid, base.GetUniqueId(@enum));
            Assert.Equal(enumMember, base.GetEnumMember(@enum));
            Assert.Equal(sortOrderDifferentDefault, base.GetEnumSortOrder(@enum, testDefaultSort));

            if (@enum == UserEntityTypes.ErrorAccount)
            {
                entity.Email = keyValue.FirstOrDefault(p => p.Key == "Email")?.Value;
                entity.FirstName = keyValue.FirstOrDefault(p => p.Key == "FirstName")?.Value;
                entity.LastName = keyValue.FirstOrDefault(p => p.Key == "LastName")?.Value;


                Assert.Equal(fullName, @enum.ToString());
                Assert.Null(base.GetEnumDescription(@enum));
                Assert.Equal(OltCommonDefaults.SortOrder, sortOrder);
                Assert.Null(entity.Username);
                Assert.Null(uid);
                Assert.Null(enumMember);
            }
            else
            {
                entity.Email = keyValue.First(p => p.Key == "Email").Value;
                entity.FirstName = keyValue.First(p => p.Key == "FirstName")?.Value;
                entity.LastName = keyValue.First(p => p.Key == "LastName")?.Value;

                Assert.Equal(sortOrder, base.GetEnumSortOrder(@enum));
                Assert.Equal(fullName, base.GetEnumDescription(@enum));
            }

            Assert.Equal(entity.Email, keyValue.FirstOrDefault(p => p.Key == "Email")?.Value);
            Assert.Equal(entity.FirstName, keyValue.FirstOrDefault(p => p.Key == "FirstName")?.Value);
            Assert.Equal(entity.LastName, keyValue.FirstOrDefault(p => p.Key == "LastName")?.Value);

            Results.Add(entity);
        }

        

    }
}
