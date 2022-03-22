using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRule : OltRuleAction<SimpleRequest>, ITestRule
    {

        public override IOltResult Execute(SimpleRequest request)
        {
            return Success;
        }

    }
}