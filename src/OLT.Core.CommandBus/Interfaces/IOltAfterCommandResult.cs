using System;
using System.Collections.Generic;

namespace OLT.Core
{
    public interface IOltAfterCommandResult
    {
        bool Success { get; }
        List<Exception> Errors { get; }
    }
}