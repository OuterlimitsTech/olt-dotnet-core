using System.Reflection.Emit;

namespace OLT.Core
{
    /// <summary>
    /// General validation error message class
    /// </summary>
    public class OltValidationError : IOltValidationError
    {

        private string? _message;

        public OltValidationError(string message)
        {
            this.Message = message;
        }

        public string? Message 
        {
            get => _message;
            set
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
                _message = value;
            }
        }
    }
}
