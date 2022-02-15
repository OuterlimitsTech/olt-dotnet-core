using System;

namespace OLT.Core
{

    /// <summary>
    /// Validtion error message
    /// </summary>
    public class OltValidationError : IOltValidationError
    {

        private string _message;

        public OltValidationError(string message)
        {
            this.Message = message;
        }

        public string Message 
        {
            get => _message;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _message = value;
            }
        }
    }
}
