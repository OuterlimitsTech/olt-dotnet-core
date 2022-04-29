using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test2Rule : OltActionRule<Test2Rule>
    {
        //public override bool RequiresDbTransaction => true;

        protected override Task RunRuleAsync()
        {
            var parameter = GetParameter<TestParameter>();
            var name = parameter.Name;
            return Task.CompletedTask;
        }
    }
}
