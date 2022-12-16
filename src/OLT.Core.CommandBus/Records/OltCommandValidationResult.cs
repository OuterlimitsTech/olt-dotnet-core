using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public record OltCommandValidationResult(List<string> Errors) : IOltCommandValidationResult
    {
        public bool Valid => !Errors.Any();

        public static OltCommandValidationResult Allow()
        {
            return new OltCommandValidationResult(new List<string>());
        }

        public static OltCommandValidationResult DontAllow(List<string> errors)
        {
            return new OltCommandValidationResult(errors);
        }

        public static OltCommandValidationResult FromResult(params ValidationResult[] results)
        {
            var errors = results.SelectMany(s => s.Errors.Select(s => s.ErrorMessage)).ToList();
            return new OltCommandValidationResult(errors);
        }

        public OltValidationException ToException(string message = "Unable to process request")
        {
            return new OltValidationException(Errors.Select(error => new OltValidationError(error)), message);
        }
    }
}