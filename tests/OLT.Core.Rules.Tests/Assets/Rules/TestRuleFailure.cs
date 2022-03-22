using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRuleFailure : OltRuleAction<SimpleModelRequest>, ITestRule
    {
        public override IOltResult Execute(SimpleModelRequest request)
        {
            throw Failure("This is a test");
        }
    }
}