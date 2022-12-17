using System;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public interface IOltRule
    {
        string RuleName { get; }
    }
}