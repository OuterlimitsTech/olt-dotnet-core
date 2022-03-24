using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Rules;

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

    }
}