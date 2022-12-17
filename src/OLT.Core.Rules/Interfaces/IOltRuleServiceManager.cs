using System;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public interface IOltRuleServiceManager : IOltServiceManager
    {
        TService GetService<TService>() where TService : notnull;
    }
}