using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class RuleContextTests : BaseUnitTests
    {

        [Fact]
        public void ServiceManagerTests()
        {
            using (var provider = BuildProvider())
            {
                var manager = provider.GetService<IOltRuleServiceManager>();
                Assert.NotNull(manager as OltRuleServiceManager);
                Assert.NotNull(manager.GetService<ITestRuleService>());
                Assert.Throws<InvalidOperationException>(() => manager.GetService<ITestRuleBogusService>());
            }


            using (var manager = new OltRuleServiceManager(null))
            {
                Assert.Throws<ArgumentNullException>(() => manager.GetService<ITestRuleService>());
            }

        }

        [Fact]
        public void ContextTests()
        {
            using (var provider = BuildProvider())
            {
                var ruleContext = provider.GetService<IOltRuleContext>();
                Assert.NotNull(ruleContext as TestRuleContext);
                Assert.NotNull(ruleContext.ServiceManager.GetService<ITestRuleService>());
                Assert.NotNull(ruleContext as ITestRuleContext);
                Assert.Throws<InvalidOperationException>(() => ruleContext.ServiceManager.GetService<ITestRuleBogusService>());                
            }


        }
    }
}
