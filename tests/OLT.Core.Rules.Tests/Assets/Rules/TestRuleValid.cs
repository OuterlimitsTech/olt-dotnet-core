namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRuleValid : OltRuleAction<SimpleRequest>, ITestRule
    {
        public override IOltResult Execute(SimpleRequest request)
        {
            return Valid;
        }

    }
}