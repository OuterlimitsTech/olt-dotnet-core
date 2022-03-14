using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OLT.Core;
using OLT.EF.Core.Tests.Assets;
using System;

namespace OLT.EF.Core.Tests
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
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
                })
                .AddDbContextPool<UnitTestAlternateContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
                });



            services.AddScoped<IOltDbAuditUser, DbAuditUserService>();
            return services.BuildServiceProvider();
        }
    }
}