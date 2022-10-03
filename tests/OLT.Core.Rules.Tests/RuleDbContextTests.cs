using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class Test1RuleTests : BaseUnitTests
    {

        [Fact]
        public async Task DbContextTests()
        {
            using (var provider = BuildProvider(true))
            {
                var rule = new Test1Rule();
                rule.WithService(provider.GetService<ITestRuleService>());
                await Assert.ThrowsAsync<ArgumentNullException>("context", () => rule.ExecuteAsync<UnitTestContext>(null));
            }

            using (var provider = BuildProvider(true))
            {
                var rule = new Test1Rule();
                await Assert.ThrowsAsync<OltRuleMissingServiceException<ITestRuleService>>(() => rule.ExecuteAsync(provider.GetService<UnitTestContext>()));
            }


            using (var provider = BuildProvider(true))
            {
                var rule = new Test1Rule();
                rule.WithService(provider.GetService<ITestRuleService>());
                Func<Task> func = () => rule.ExecuteAsync(provider.GetService<UnitTestContext>());
                await func.Should().NotThrowAsync<OltRuleException>();
            }
        }

        [Fact]
        public async Task TranTests()
        {
            using (var provider = BuildProvider())
            {
                var rule = new Test1Rule();
                rule.WithService(provider.GetService<ITestRuleService>());
                var error = await Assert.ThrowsAsync<AggregateException>(() => rule.ExecuteAsync(null));
                Assert.IsType<OltRuleMissingTransactionException>(error.InnerException);
            }          

            using (var provider = BuildProvider())
            {
                var rule = new Test1Rule();
                using (var tran = new MockTran())
                {
                    await Assert.ThrowsAsync<OltRuleMissingServiceException<ITestRuleService>>(() => rule.ExecuteAsync(tran));
                }
            }

            using (var provider = BuildProvider())
            {
                var rule = new Test1Rule();
                rule.WithService(provider.GetService<ITestRuleService>());
                using (var tran = new MockTran())
                {
                    Func<Task> func = () => rule.ExecuteAsync(tran);
                    await func.Should().NotThrowAsync<OltRuleException>();
                }
            }
        }
    }
}