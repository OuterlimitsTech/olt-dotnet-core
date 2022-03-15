using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Linq;
using Xunit;

namespace OLT.EF.Core.Tests
{
    public class ContextExtensionsTests : BaseUnitTests
    {
        [Fact]
        public void GetTableNameTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestContext>();
                Assert.Equal($"{context.DefaultSchema}.{nameof(PersonEntity)}", OltContextExtensions.GetTableName<PersonEntity>(context));
                Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.GetTableName<PersonEntity>(null));

                var context2 = provider.GetService<UnitTestAlternateContext>();
                Assert.Equal($"{nameof(PersonEntity)}", OltContextExtensions.GetTableName<PersonEntity>(context2));
            }
        }

        [Fact]
        public void GetColumnsTests()
        {
            using (var provider = BuildProvider())
            {
                const string expected = "PersonEntityId";
                var context = provider.GetService<UnitTestContext>();
                
                var columns = OltContextExtensions.GetColumns<PersonEntity>(context).ToList();
                columns.ForEach(col =>
                {                    
                    //Logger.Debug(col.Name);
                });
                Assert.Collection(columns,
                    item => Assert.Equal(item.Name, expected),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.ActionCode)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.CreateDate)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.CreateUser)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.DeletedBy)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.DeletedOn)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.ModifyDate)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.ModifyUser)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.NameFirst)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.NameLast)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.NameMiddle)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.StatusTypeId)}"),
                    item => Assert.Equal(item.Name, $"{nameof(PersonEntity.UniqueId)}")
                    );


                Assert.Throws<ArgumentNullException>("dbContext", () => OltContextExtensions.GetColumns<PersonEntity>(null));

            }
        }

    }
}