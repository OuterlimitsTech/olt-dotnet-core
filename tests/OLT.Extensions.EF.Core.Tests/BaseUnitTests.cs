using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.Extensions.EF.Core.Tests.Assets;
using System;
using Serilog;

namespace OLT.Extensions.EF.Core.Tests
{
    public abstract class BaseUnitTests
    {

        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();

            services
                //.AddLogging(config => config.AddConsole())
                .AddLogging(loggingBuilder =>  loggingBuilder.AddSerilog(dispose: true))
                .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}", opt => opt.EnableNullChecks());
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();

                    var options = optionsBuilder.Options as DbContextOptions<UnitTestContext>;
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureDeleted();
                    }
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureCreated();
                    }

                });

            services.AddScoped<IOltDbAuditUser, DbAuditUserService>();
            return services.BuildServiceProvider();
        }

    }
}