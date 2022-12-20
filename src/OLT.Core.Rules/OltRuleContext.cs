using System;

namespace OLT.Core
{
    [Obsolete("Move to OltCommandBus")]
    public abstract class OltRuleContext : OltDisposable, IOltRuleContext
    {
        protected OltRuleContext(IOltRuleServiceManager serviceManager)
        {
            ServiceManager = serviceManager;
        }

        public IOltRuleServiceManager ServiceManager { get; }
    }
}