using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test3Rule : OltActionRule<Test3Rule>
    {
        public bool ThrowError { get; set; } = true;

        protected override Task RunRuleAsync()
        {
            var service = GetService<ITestRuleService>(ThrowError);
            var parameter = GetParameter<TestParameter>(ThrowError);

            service?.TestMethod();

            return Task.CompletedTask;
        }
    }
}
