using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using Serilog;
using System;

namespace OLT.EF.Core.Tests
{

    public abstract class BaseUnitTests
    {
        protected ServiceProvider BuildProviderWithLogging()
        {
            var services = new ServiceCollection();

            services
                .AddLogging(config => config.AddConsole())
                //.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false))
                .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}", opt => opt.EnableNullChecks());
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.LogTo(Console.WriteLine);
                    //optionsBuilder.LogTo(message => Debug.Writeline(message));

                    var options = optionsBuilder.Options as DbContextOptions<UnitTestContext>;
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureDeleted();
                    }
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureCreated();
                    }

                })
                .AddDbContextPool<UnitTestAlternateContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.LogTo(Console.WriteLine);
                });

            services.AddScoped<IOltDbAuditUser, DbAuditUserService>();

            return services.BuildServiceProvider();
        }
        protected ServiceProvider BuildProvider()
        {
            //var loggerMock = new Mock<ILogger<OltDbContext<UnitTestContext>>>();

            var services = new ServiceCollection();

            services
                //.AddLogging(config => config.AddConsole())
                //.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false))
                .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}", opt => opt.EnableNullChecks());
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.LogTo(Console.WriteLine);
                    //optionsBuilder.LogTo(message => Debug.Writeline(message));

                    var options = optionsBuilder.Options as DbContextOptions<UnitTestContext>;
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureDeleted();
                    }
                    using (var context = new UnitTestContext(options))
                    {
                        context.Database.EnsureCreated();
                    }

                })
                .AddDbContextPool<UnitTestAlternateContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.LogTo(Console.WriteLine);
                });

            services.AddScoped<IOltDbAuditUser, DbAuditUserService>();

            return services.BuildServiceProvider();
        }

    }
}