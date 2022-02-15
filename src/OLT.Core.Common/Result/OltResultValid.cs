using System.Collections.Generic;

namespace OLT.Core
{
    /// <summary>
    /// General Valid Result
    /// </summary>
    public class OltResultValid : IOltResultValidation
    {
        public virtual bool Success => true;
        public bool Invalid => false;
        public List<IOltValidationError> Results => new List<IOltValidationError>();
    }
}