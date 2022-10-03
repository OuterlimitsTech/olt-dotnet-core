using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{

    public class TestRuleService : OltCoreService<TestCoreServiceManager>, ITestRuleService
    {
        public TestRuleService(IOltServiceManager serviceManager) : base(serviceManager)
        {
        }

        public bool TestMethod()
        {
            return true;
        }
    }
}
