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

        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IOltRule, TestRuleFailure>();
            services.AddScoped<IOltRule, TestRuleBadRequest>();
            services.AddScoped<IOltRule, TestRuleInValid>();
            services.AddScoped<IOltRule, TestRule>();            
            services.AddScoped<IOltRule, TestRuleValid>();
            services.AddScoped<IOltRule, TestRuleMulipleInterface>();
            services.AddScoped<IOltRuleManager, OltRuleManager>();

            return services.BuildServiceProvider();
        }


        protected ServiceProvider BuildProvider2()
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

            services.AddScoped<IOltServiceManager, TestRuleServiceManager>();
            services.AddScoped<ITestRuleService, TestRuleService>();


            return services.BuildServiceProvider();
        }

    }
}