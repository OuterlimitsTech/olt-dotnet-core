using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using OLT.EF.Core.SeedHelpers.Csv.Tests.Assets;
using System;
using Xunit;

namespace OLT.EF.Core.SeedHelpers.Csv.Tests
{
    public class UnitTests
    {

        [Fact]
        public void ConfigurationTest()
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
                    var entityTypeBuilder = builder.Entity<PersonTypeCodeEntity>();


                    var config = new PersonTypeConfiguration();
                    config.Configure(entityTypeBuilder);


                    config.Results.Should().HaveCount(3);

                }
            }
        }


        [Fact]
        public void NotACsvConfigurationTest()
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
                    var entityTypeBuilder = builder.Entity<PersonTypeCodeEntity>();


                    var config = new NotACsvResourceConfiguration();
                    Action act = () => config.Configure(entityTypeBuilder);

                    act.Should().Throw<CsvHelper.HeaderValidationException>();
                }
            }
        }

        [Fact]
        public void InvalidConfigurationTest()
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
                    var entityTypeBuilder = builder.Entity<PersonTypeCodeEntity>();


                    var config = new InvalidResourceConfiguration();
                    Action act = () => config.Configure(entityTypeBuilder);

                    act.Should().Throw<System.IO.FileNotFoundException>();

                }
            }
        }
    }
}