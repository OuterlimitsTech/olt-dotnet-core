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
        public void CodeValueTypeConfigurationEnumTest()
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
                    var entityTypeBuilder = builder.Entity<PersonTypeCodeEntity>();


                    var config = new PersonTypeConfiguration();
                    config.Configure(entityTypeBuilder);


                    config.Results.Should().HaveCount(3);

                }
            }
        }
    }
}