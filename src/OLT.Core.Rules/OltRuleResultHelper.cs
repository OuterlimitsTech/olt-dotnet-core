using OLT.Constants;

namespace OLT.Core
{
    public static class OltRuleResultHelper
    {
        public static IOltRuleResult Success => new OltRuleResultValid();
        public static IOltRuleResult Invalid => new OltRuleResultInvalid(new OltValidationError(OltRuleDefaults.InvalidMessage));
    }
}