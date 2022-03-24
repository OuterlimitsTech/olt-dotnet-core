namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRuleInValid : OltRuleAction<SimpleRequest>, ITestRuleSimpleRequest
    {
        public override IOltRuleResult Execute(SimpleRequest request)
        {
            return InValid;
        }

    }
}