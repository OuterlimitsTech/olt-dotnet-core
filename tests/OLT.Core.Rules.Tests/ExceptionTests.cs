using Microsoft.EntityFrameworkCore.Storage;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class ExceptionTests
    {

        [Fact]
        public void MissingTransactionExceptionTest()
        {
            var rule = new Test1RuleBuilder();
            var ex = new OltRuleMissingTransactionException(rule);            
            Assert.Equal($"{rule.RuleName} requires {nameof(IDbContextTransaction)}", ex.Message);
        }

        [Fact]
        public void MissingServiceExceptionTest()
        {
            var rule = new Test1RuleBuilder();
            var ex = new OltRuleMissingServiceException<ITestRuleService>(rule);
            Assert.Equal($"{rule.RuleName} requires {typeof(ITestRuleService).FullName}", ex.Message);
        }

        [Fact]
        public void MissingParameterExceptionTest()
        {
            var rule = new Test1RuleBuilder();
            var ex = new OltRuleMissingParameterException<TestParameter>(rule);
            Assert.Equal($"{rule.RuleName} requires {typeof(TestParameter).FullName}", ex.Message);
        }
    }
}
