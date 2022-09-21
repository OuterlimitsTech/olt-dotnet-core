namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class TestRuleContext : OltRuleContext, ITestRuleContext
    {
        public TestRuleContext(IOltRuleServiceManager serviceManager) : base(serviceManager)
        {
        }
        
    }
}
