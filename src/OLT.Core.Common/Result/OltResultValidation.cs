using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{

    /// <summary>
    /// Validation Result container list of errors
    /// </summary>
    public class OltResultValidation : IOltResultValidation
    {
        public OltResultValidation() { }

        public OltResultValidation(OltValidationError validationError)
        {
            Results.Add(validationError);
        }

        public OltResultValidation(string errorMessage)
        {
            Results.Add(new OltValidationError(errorMessage));
        }

        public bool Invalid => Results.Any();
        public virtual List<IOltValidationError> Results { get; } = new List<IOltValidationError>();
        public virtual bool Success => !Results.Any();
    }
}