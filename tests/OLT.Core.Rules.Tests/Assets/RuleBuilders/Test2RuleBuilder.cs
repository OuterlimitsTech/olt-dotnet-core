using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test2RuleBuilder : OltRuleBuilder
    {
        public override bool RequiresDbTransaction => true;

        protected override Task<IOltRuleResult> RunRuleAsync()
        {
            var parameter = GetParameter<TestParameter>();
            var name = parameter.Name;
            return Task.FromResult(Success);
        }
    }
}
