﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
   
    public class RuleBuilderTests : BaseUnitTests
    {
        [Fact]
        public async Task GeneralTests()
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


            using (var provider = BuildProvider())
            {
                var rule = new Test2Rule();                
                using (var tran = new MockTran())
                {
                    Func<Task> func = () => rule.ExecuteAsync(tran);
                    await func.Should().ThrowAsync<OltRuleMissingParameterException<TestParameter>>();
                }
            }

            using (var provider = BuildProvider())
            {
                var rule = new Test4Rule();
                rule.WithValue("intValue", rule.intValue)
                    .WithValue("strValue", rule.strValue);

                using (var tran = new MockTran())
                {
                    await rule.ExecuteAsync(tran);
                }
            }

        }


        [Fact]
        public async Task DependentRuleTests()
        {

            using (var provider = BuildProvider())
            {
                var service = provider.GetService<ITestRuleService>();
                var param = new TestParameter();
                var rule1 = new Test1Rule()
                    .WithDependentRule(new Test2Rule().WithParameter(param), OltDependentRuleRunTypes.RunBefore)
                    .WithDependentRule(new Test3Rule()
                                            .WithParameter(param)
                                            .WithService(service), OltDependentRuleRunTypes.RunAfter)
                    .WithService(service);
                
                using (var tran = new MockTran())
                {
                    Func<Task> func = () => rule1.ExecuteAsync(tran);
                    await func.Should().NotThrowAsync<OltRuleException>();
                }
            }


        }
    }
}