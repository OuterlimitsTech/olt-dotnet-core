using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OLT.Core.CommandBus.Tests.Assets;
using OLT.Core.CommandBus.Tests.Assets.EfCore;
using OLT.Core.CommandBus.Tests.Assets.Handlers;
using System;

namespace OLT.Core.CommandBus.Tests
{
    public abstract class BaseUnitTests
    {

        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();

            services
                .AddLogging(config => config.AddConsole())
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

            services.AddScoped<IOltCommandBus, UnitTestCommandBus>();
            services.AddScoped<IOltCommandHandler, DuplicateHandlerCommandHandler>();
            services.AddScoped<IOltCommandHandler, DuplicateHandlerSecondCommandHandler>();
            services.AddScoped<IOltCommandHandler, UserEntityCommandHandler>();
            services.AddScoped<IOltCommandHandler, SimpleCommandHandler>();
            services.AddScoped<IOltCommandHandler, TestPersonCommandHandler>();
            return services.BuildServiceProvider();
        }

    }
}