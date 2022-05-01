using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test1Rule : OltActionRule<Test1Rule>
    {
        protected override Task RunRuleAsync()
        {
            var service = GetService<ITestRuleService>();
            service.TestMethod();
            return Task.CompletedTask;
        }
    }
}
