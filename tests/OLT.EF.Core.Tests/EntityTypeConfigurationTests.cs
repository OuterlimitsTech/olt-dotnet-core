using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using OLT.EF.Core.Tests.Assets.Entites;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using OLT.EF.Core.Tests.Assets.EntityTypeConfigurations;
using OLT.EF.Core.Tests.Assets.EntityTypeConfigurations.InvalidTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;


namespace OLT.EF.Core.Tests
{
    public class EntityTypeConfigurationTests
    {

        [Fact]
        public void EntityTypeConfigurationTest()
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
                    var entityTypeBuilder = builder.Entity<UserEntity>();

                    Assert.True(entityTypeBuilder.Property(p => p.FirstName).Metadata.IsColumnNullable());

                    var config = new UserEntityTestConfiguration();
                    config.Configure(entityTypeBuilder);

                    Assert.False(entityTypeBuilder.Property(p => p.FirstName).Metadata.IsColumnNullable());
                }
            }
        }

        [Fact]
        public void EntityTypeConfigurationEnumTest()
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
                    var entityTypeBuilder = builder.Entity<UserEntity>();

                    Assert.True(entityTypeBuilder.Property(p => p.LastName).Metadata.IsColumnNullable());

                    var config = new UserEntityTestEnumConfiguration();
                    config.Configure(entityTypeBuilder);

                    Assert.False(entityTypeBuilder.Property(p => p.LastName).Metadata.IsColumnNullable());

                    config.Results.Should().HaveCount(3);

                }
            }
        }


        [Fact]
        public void CodeValueTypeConfigurationEnumTest()
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
                    var entityTypeBuilder = builder.Entity<UserType>();


                    var config = new UserTypeEntityTestEnumConfiguration();
                    config.Configure(entityTypeBuilder);


                    config.Results.Should().HaveCount(3);

                }
            }
        }



        [Fact]
        public void ExceptionMinValueTest()
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
                    var entityTypeBuilder = builder.Entity<UserType>();


                    var config = new NegativeEnumConfiguration();
                    var exception = Assert.Throws<OltException>(() => config.Configure(entityTypeBuilder));
                    Assert.Equal($"Enum underlying value must be greater or equal to 1", exception.Message);
                }
            }
        }

        [Fact]
        public void ExceptionLongEnumTest()
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
                    var entityTypeBuilder = builder.Entity<UserType>();


                    var config = new LongEnumConfiguration();
                    var exception = Assert.Throws<InvalidCastException>(() => config.Configure(entityTypeBuilder));
                    Assert.Equal($"Type '{typeof(LongValueTypes).AssemblyQualifiedName}' must be of type uint, int", exception.Message);
                }
            }
        }
        
    }
}
