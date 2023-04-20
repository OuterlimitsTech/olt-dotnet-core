using System.Collections.Generic;

namespace OLT.Core
{
    public class OltValidationException : OltException
    {
        public readonly IEnumerable<IOltValidationError> Results;

        public OltValidationException(IEnumerable<IOltValidationError> results, string errorMessage = "Please correct the validation errors") : base(errorMessage)
        {
            this.Results = results;
        }        
    }
}
