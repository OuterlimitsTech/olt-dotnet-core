using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using OLT.Core.Rules.Tests.Assets.Rules;
using System;

namespace OLT.Core.Rules.Tests
{
    public abstract class BaseUnitTests
    {

        //protected ServiceProvider BuildProvider()
        //{
        //    var services = new ServiceCollection();
        //    //services.AddScoped<IOltRule, TestRuleFailure>();
        //    //services.AddScoped<IOltRule, TestRuleBadRequest>();
        //    //services.AddScoped<IOltRule, TestRuleInValid>();
        //    //services.AddScoped<IOltRule, TestRule>();            
        //    //services.AddScoped<IOltRule, TestRuleValid>();
        //    //services.AddScoped<IOltRule, TestRuleMulipleInterface>();
        //    //services.AddScoped<IOltRuleManager, OltRuleManager>();

        //    return services.BuildServiceProvider();
        //}


        protected ServiceProvider BuildProvider(bool withDbContext = false)
        {
            var services = new ServiceCollection();

            //.AddLogging(config => config.AddConsole())
            //.AddAutoMapper(this.GetType().Assembly)

            //services
            //    .AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
            //    {
            //        optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}");
            //        optionsBuilder.EnableSensitiveDataLogging();
            //        optionsBuilder.EnableDetailedErrors();

            //    });

            
            services.AddScoped<IOltServiceManager, TestCoreServiceManager>();
            services.AddScoped<IOltRuleServiceManager, OltRuleServiceManager>();
            services.AddScoped<ITestRuleService, TestRuleService>();
            services.AddScoped<ITestRuleContext, TestRuleContext>();
            services.AddScoped<IOltRuleContext>(services => services.GetRequiredService<ITestRuleContext>());
            services.AddScoped<IOltRuleServiceManager, OltRuleServiceManager>();

            if (withDbContext)
            {
                services.AddDbContextPool<UnitTestContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: $"UnitTest_EFCore_{Guid.NewGuid()}", opt => opt.EnableNullChecks());
                    optionsBuilder.EnableSensitiveDataLogging();
                    optionsBuilder.EnableDetailedErrors();

                });
            }

            return services.BuildServiceProvider();
        }

    }
}