using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace OLT.EF.Core.Tests
{

    public class ModelBuilderExtensionTests 
    {
        [Fact]
        public void EntitiesOfTypeTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            
            var expected = OltSystemReflectionExtensions.GetAllImplements<IOltEntityId>(new List<Assembly>() { this.GetType().Assembly }).Count();
            var count = 0;
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);
                    OltModelBuilderExtensions.EntitiesOfType<IOltEntityId>(builder, opt =>
                    {
                        count++;
                    });

                    Action<EntityTypeBuilder> nullAction = null;

                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderExtensions.EntitiesOfType<IOltEntityId>(null, opt => {  }));
                    Assert.Throws<ArgumentNullException>("buildAction", () => OltModelBuilderExtensions.EntitiesOfType<IOltEntityId>(builder, nullAction));

                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderExtensions.EntitiesOfType(null, typeof(IOltEntityId), opt => { }));
                    Assert.Throws<ArgumentNullException>("type", () => OltModelBuilderExtensions.EntitiesOfType(builder, null, opt => { }));
                    Assert.Throws<ArgumentNullException>("buildAction", () => OltModelBuilderExtensions.EntitiesOfType(builder, typeof(IOltEntityId), null));                    
                }
            }
            Assert.Equal(expected, count);

        }


        [Fact]
        public void ApplyGlobalFilterTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);
                    builder.Entity<CodeTableEntity>().HasDiscriminator();

                    OltModelBuilderExtensions.ApplyGlobalFilters<IOltEntityDeletable>(builder, p => p.DeletedBy == null);

                    Assert.NotNull(builder.Entity<CodeTableEntity>().Metadata.GetQueryFilter());
                    Assert.Null(builder.Entity<StatusTypeCodeTableEntity>().Metadata.GetQueryFilter());

                    Assert.Contains("DeletedBy == null", builder.Entity<PersonEntity>().Metadata.GetQueryFilter().ToString());
                    Assert.Contains("DeletedBy == null", builder.Entity<CodeTableEntity>().Metadata.GetQueryFilter().ToString());
                    
                    

                    Expression<Func<IOltEntityDeletable, bool>> nullAction = null;
                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderExtensions.ApplyGlobalFilters<IOltEntityDeletable>(null, p => p.DeletedBy == null));
                    Assert.Throws<ArgumentNullException>("expression", () => OltModelBuilderExtensions.ApplyGlobalFilters<IOltEntityDeletable>(builder, nullAction));

                }
            }

            Assert.True(true);

        }


        [Fact]
        public void SoftDeleteGlobalFilterTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<UnitTestContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<UnitTestContext>())
                {
                    var conventionSet = ConventionSet.CreateConventionSet(context);
                    var builder = new ModelBuilder(conventionSet);

                    OltModelBuilderExtensions.SetSoftDeleteGlobalFilter(builder);

                    Assert.Contains("DeletedOn == null", builder.Entity<PersonEntity>().Metadata.GetQueryFilter().ToString());
                    Assert.Contains("DeletedOn == null", builder.Entity<StatusTypeCodeTableEntity>().Metadata.GetQueryFilter().ToString());
                    Assert.Null(builder.Entity<CodeTableEntity>().Metadata.GetQueryFilter());

                    
                    Assert.Throws<ArgumentNullException>("modelBuilder", () => OltModelBuilderExtensions.SetSoftDeleteGlobalFilter(null));

                }
            }

            Assert.True(true);

        }
    }
}