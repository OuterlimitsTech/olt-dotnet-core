using System.Linq;
using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRuleMulipleInterface : OltRuleAction<SimpleModelRequest>, ITestRuleSimpleModelRequest, ITestRuleSimpleRequest
    {

        public override IOltRuleResult Execute(SimpleModelRequest request)
        {
            if (!string.IsNullOrEmpty(request.Value.Name))
            {
                return Success;
            }

            throw Failure("Nothing to Process");
        }

        public IOltRuleResult Execute(SimpleRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.ValueRequest))
            {
                return Success;
            }
            throw Failure("Nothing to Process", new System.Exception("Invalid Request"));
        }
    }
}