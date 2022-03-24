using System.Collections.Generic;

namespace OLT.Core
{
    public class OltRuleResultValid : IOltRuleResult
    {
        public virtual bool Success => true;
        public virtual bool Invalid => false;
        public virtual List<IOltValidationError> Results => new List<IOltValidationError>();
    }

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