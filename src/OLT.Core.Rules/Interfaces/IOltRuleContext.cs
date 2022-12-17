using System;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public interface IOltRuleContext : IOltInjectableScoped
    {
        IOltRuleServiceManager ServiceManager { get; }
    }
}