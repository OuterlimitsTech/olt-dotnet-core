using System.Linq;
using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class TestRuleMulipleInterface : OltRuleAction<SimpleModelRequest>, ITestRule, ITestRuleSimpleRequest
    {
        //public override IOltResultValidation Validate(DoSomethingRuleContextRequest request)
        //{
        //    if (request.Context.People.Any())
        //    {
        //        return Valid;
        //    }
        //    return BadRequest("No People");
        //}

        public override IOltResult Execute(SimpleModelRequest request)
        {
            if (!string.IsNullOrEmpty(request.Value.Name))
            {
                return Success;
            }

            throw Failure("Nothing to Process");
        }

        public IOltResult Execute(SimpleRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.ValueRequest))
            {
                return Success;
            }
            throw Failure("Nothing to Process");
        }
    }
}