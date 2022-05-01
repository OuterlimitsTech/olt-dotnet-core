using System;
using System.Collections.Generic;

namespace OLT.Core
{
    [Obsolete("Move away from Results and Throw Exceptions")]
    public class OltRuleResultValid : IOltRuleResult
    {
        public virtual bool Success => true;
        public virtual bool Invalid => false;
        public virtual List<IOltValidationError> Results => new List<IOltValidationError>();
    }

    [Obsolete("Move away from Results and Throw Exceptions")]
    public class OltRuleResultInvalid : IOltRuleResult
    {
        public OltRuleResultInvalid(IOltValidationError validationError)
        {
            Results.Add(validationError);
        }

        public virtual bool Success => false;
        public virtual bool Invalid => true;
        public virtual List<IOltValidationError> Results => new List<IOltValidationError>();
    }
}