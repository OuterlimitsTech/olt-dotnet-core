using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets;
using OLT.EF.Core.Services.Tests.Assets.Services;
using OLT.EF.Core.Tests.Assets;
using System;
using System.Collections.Generic;
using OLT.EF.Core.Services.Tests.Assets.Models.Adapters;

namespace OLT.EF.Core.Services.Tests
{

    public abstract class BaseUnitTests
    {
        

     

        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();

            services
                .AddLogging(config => config.AddConsole())                
                .AddAutoMapper(this.GetType().Assembly)
                .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}", opt => opt.EnableNullChecks());
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();

                });


            services                
                .AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>()
                .AddScoped<IOltServiceManager, OltEfCoreServiceManager>()
                .AddScoped<IContextService, ContextService>()
                .AddSingleton<IOltAdapter, UserModelAdapter>()
                .AddSingleton<IOltAdapter, UserDtoAdapter>()
                .AddScoped<IOltDbAuditUser, DbAuditUserService>();


            return services.BuildServiceProvider();
        }

    }
}
