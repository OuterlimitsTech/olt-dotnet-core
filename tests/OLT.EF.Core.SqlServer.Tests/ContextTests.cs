using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.SqlServer.Tests.Assests;
using OLT.EF.Core.SqlServer.Tests.Assests.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Core.SqlServer.Tests
{
    public class ContextTests : BaseUnitTests
    {

        [Fact]
        public void ConfigTests()
        {
            using (var provider = BuildProvider())
            {
                var context = provider.GetService<UnitTestSqlContext>();               

                Assert.Equal(1000, context.IdentitySeedValue);
                Assert.Equal(2, context.IdentityIncrementValue);
            }
        }

        [Fact]
        public void EntitiesOfTypeTests()
        {
            const int identitySeed = 1506;
            const int identityIncrement = 10;

            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestSqlContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<UnitTestSqlContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);

                    Assert.False(builder.Entity<PersonEntity>().Property(p => p.Id).Metadata.GetIdentitySeed().HasValue);
                    Assert.True(builder.Entity<PersonEntity>().Property(p => p.Id).Metadata.GetIdentityIncrement().HasValue);  //Defaults to 1
                    Assert.False(builder.Entity<UserEntity>().Property(p => p.Id).Metadata.GetIdentitySeed().HasValue);
                    Assert.True(builder.Entity<UserEntity>().Property(p => p.Id).Metadata.GetIdentityIncrement().HasValue);  //Defaults to 1

                    OltSqlModelBuilderHelper.SetIdentityColumns(builder, identitySeed, identityIncrement);


                    Assert.Equal(identitySeed, builder.Entity<PersonEntity>().Property(p => p.Id).Metadata.GetIdentitySeed());
                    Assert.Equal(identityIncrement, builder.Entity<PersonEntity>().Property(p => p.Id).Metadata.GetIdentityIncrement());
                    Assert.NotEqual(identitySeed, builder.Entity<UserEntity>().Property(p => p.Id).Metadata.GetIdentitySeed());
                    Assert.NotEqual(identityIncrement, builder.Entity<UserEntity>().Property(p => p.Id).Metadata.GetIdentityIncrement());


                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltSqlModelBuilderHelper.SetIdentityColumns(null, 1, 1));
                }
            }


        }
    }
}
