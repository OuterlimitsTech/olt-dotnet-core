using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations
{
    public enum UserTypes
    {
        NoAttributes = 123,

        [Code("TestCode", 1234)]
        [Description("Testing")]
        [UniqueId("bb07399c-6bd2-41fe-a680-2b5b86fcae8a")]
        AttributeType = 678,
    }

    public class UserTypeEntityTestEnumConfiguration : OltEntityTypeConfiguration<UserType, UserTypes>
    {
        public List<UserType> Results { get; } = new List<UserType>();

        protected override void Map(UserType entity, UserTypes @enum)
        {
            base.Map(entity, @enum);
            Results.Add(entity);

            if (@enum == UserTypes.NoAttributes)
            {
                Assert.Equal((int)UserTypes.NoAttributes, entity.Id);
                Assert.NotEqual(Guid.Empty, entity.UniqueId);
                Assert.Equal("NoAttributes", entity.Code);
                Assert.Equal("NoAttributes", entity.Name);
                Assert.Equal(DefaultSort, entity.SortOrder);
            }
            else
            {
                Assert.Equal((int)UserTypes.AttributeType, entity.Id);
                Assert.Equal("bb07399c-6bd2-41fe-a680-2b5b86fcae8a", entity.UniqueId.ToString());
                Assert.Equal("TestCode", entity.Code);
                Assert.Equal("Testing", entity.Name);
                Assert.Equal(1234, entity.SortOrder);
            }

            Assert.Equal(DefaultUsername, entity.CreateUser);
            Assert.Equal(DefaultCreateDate, entity.CreateDate);

        }
    }
}
