namespace OLT.Core
{
    /// <summary>
    /// Data or input provided fails to meet the predefined criteria or rules established by the system or application
    /// </summary>
    public class OltValidationException : OltException
    {
        public readonly IEnumerable<IOltValidationError> Results;

        public OltValidationException(IEnumerable<IOltValidationError> results, string errorMessage = "Please correct the validation errors") : base(errorMessage)
        {
            this.Results = results;
        }        
    }
}
