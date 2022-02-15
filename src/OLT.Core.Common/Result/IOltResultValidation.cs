using System.Collections.Generic;

namespace OLT.Core
{

    /// <summary>
    /// General Validation Result 
    /// </summary>
    public interface IOltResultValidation : IOltResult
    {
        bool Invalid { get; }
        List<IOltValidationError> Results { get; }
    }
}