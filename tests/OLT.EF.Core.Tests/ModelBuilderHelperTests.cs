using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace OLT.EF.Core.Tests
{
    public class ModelBuilderHelperTests
    {
        [Fact]
        public void EntityIdColumnNameTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();
            
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);

                    OltModelBuilderHelper.EntityIdColumnName(builder);

                    var entity = builder.Entity<PersonEntity>().Metadata;
                    var prop = builder.Entity<PersonEntity>().Property(p => p.Id);
                    var result = prop.Metadata.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema()));
                    Assert.Equal("PersonEntityId", result);

                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderHelper.EntityIdColumnName(null!));
                }
            }
        }

        [Fact]
        public void DisableCascadeDeleteTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);
                    Assert.True(OltModelBuilderHelper.GetAllCascadeDelete(builder).Any());  //Check to make sure we have some
                    OltModelBuilderHelper.RestrictDeleteBehavior(builder); //Should disable all the Cascade
                    Assert.False(OltModelBuilderHelper.GetAllCascadeDelete(builder).Any());  //There should not be any now

                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderHelper.GetAllCascadeDelete(null!));
                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderHelper.RestrictDeleteBehavior(null!));
                }
            }
        }

        [Fact]
        public void DisableUnicodeProperties()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);

                    Assert.True(OltModelBuilderHelper.GetAllUnicodeProperties(builder).Any());  //Check to make sure we have some
                    OltModelBuilderHelper.DisableUnicodeProperties(builder); //Should disable all unicode string properties
                    Assert.False(OltModelBuilderHelper.GetAllUnicodeProperties(builder).Any());  //There should not be any now

                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderHelper.GetAllUnicodeProperties(null!));
                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderHelper.DisableUnicodeProperties(null!));
                }
            }
        }
    }
}