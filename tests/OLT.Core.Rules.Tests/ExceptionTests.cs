using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class ExceptionTests : BaseUnitTests
    {

        [Fact]
        public void MissingTransactionExceptionTest()
        {
            var rule1 = new Test1Rule();
            var ex = new OltRuleMissingTransactionException(rule1);
            var expectedMsg = $"{rule1.RuleName} requires {nameof(IDbContextTransaction)}";
            Assert.Equal(expectedMsg, ex.Message);        }

        [Fact]
        public async Task MissingServiceExceptionTest()
        {
            var rule1 = new Test1Rule();
            var ex = new OltRuleMissingServiceException<ITestRuleService>(rule1);
            Assert.Equal($"{rule1.RuleName} requires {typeof(ITestRuleService).FullName}", ex.Message);


            var rule3 = new Test3Rule();
            using (var tran = new MockTran())
            {
                Func<Task> func = () => rule3.ExecuteAsync(tran);
                await func.Should().ThrowAsync<OltRuleMissingServiceException<ITestRuleService>>();
                rule3.ThrowError = false;
                await func.Should().NotThrowAsync<OltRuleMissingServiceException<ITestRuleService>>();
            }
        }

        [Fact]
        public async Task MissingParameterExceptionTest()
        {
            var rule1 = new Test1Rule();
            var ex = new OltRuleMissingParameterException<TestParameter>(rule1);
            Assert.Equal($"{rule1.RuleName} requires {typeof(TestParameter).FullName}", ex.Message);


            using (var provider = BuildProvider())
            {
                var rule3 = new Test3Rule().WithService(provider.GetService<ITestRuleService>());
                using (var tran = new MockTran())
                {
                    Func<Task> func = () => rule3.ExecuteAsync(tran);
                    await func.Should().ThrowAsync<OltRuleMissingParameterException<TestParameter>>();
                    rule3.ThrowError = false;
                    await func.Should().NotThrowAsync<OltRuleMissingParameterException<TestParameter>>();
                }
            }           

        }
    }
}
