using OLT.Constants;
using System;

namespace OLT.Core
{
    [Obsolete("Move to OltActionRule")]
    public static class OltRuleResultHelper
    {
        [Obsolete("Move to OltActionRule")]
        public static IOltRuleResult Success => new OltRuleResultValid();
        [Obsolete("Move to OltActionRule")]
        public static IOltRuleResult Invalid => new OltRuleResultInvalid(new OltValidationError(OltRuleDefaults.InvalidMessage));        
    }
}