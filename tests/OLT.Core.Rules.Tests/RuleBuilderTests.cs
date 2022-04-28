using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
   
    public class RuleBuilderTests : BaseUnitTests
    {
        [Fact]
        public async Task Test1RuleBuilderTest()
        {
            using (var provider = BuildProvider2())
            {
                var rule = new Test1RuleBuilder();
                rule.WithService(provider.GetService<ITestRuleService>());
                var error = await Assert.ThrowsAsync<AggregateException>(() => rule.ExecuteAsync());
                Assert.IsType<OltRuleMissingTransactionException>(error.InnerException);
            }


            using (var provider = BuildProvider2())
            {
                var rule = new Test1RuleBuilder();
                using (var tran = new MockTran())
                {
                    await Assert.ThrowsAsync<OltRuleMissingServiceException<ITestRuleService>>(() => rule.ExecuteAsync(tran));
                }
            }


            using (var provider = BuildProvider2())
            {
                var rule = new Test1RuleBuilder();
                rule.WithService(provider.GetService<ITestRuleService>());
                using (var tran = new MockTran())
                {
                    var result = await rule.ExecuteAsync(tran);
                    Assert.True(result.Success);
                }
            }


            using (var provider = BuildProvider2())
            {
                var rule = new Test2RuleBuilder();
                rule.WithService(provider.GetService<ITestRuleService>());
                using (var tran = new MockTran())
                {
                    await Assert.ThrowsAsync<OltRuleMissingParameterException<TestParameter>>(() => rule.ExecuteAsync(tran));                    
                }
            }

            using (var provider = BuildProvider2())
            {
                var rule = new Test2RuleBuilder();
                rule.WithService(provider.GetService<ITestRuleService>()).WithParameter(new TestParameter());
                using (var tran = new MockTran())
                {
                    var result = await rule.ExecuteAsync(tran);
                    Assert.True(result.Success);
                }
            }
        }



    }
}