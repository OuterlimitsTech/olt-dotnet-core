using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test1RuleBuilder : OltRuleBuilder
    {
        public override bool RequiresDbTransaction => true;

        protected override Task<IOltRuleResult> RunRuleAsync()
        {
            var service = GetService<ITestRuleService>();
            service.TestMethod();
            return Task.FromResult(Success);
        }
    }
}
