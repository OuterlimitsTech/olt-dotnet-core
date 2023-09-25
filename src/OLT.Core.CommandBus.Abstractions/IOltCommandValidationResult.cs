using System.Collections.Generic;

namespace OLT.Core
{
    public interface IOltCommandValidationResult
    {
        bool Valid { get; }
        List<string> Errors { get; }
        OltValidationException ToException(string message = "Unable to process request");
    }
}