namespace OLT.Core
{
    public abstract class OltRuleContext : OltDisposable, IOltRuleContext
    {
        protected OltRuleContext(IOltRuleServiceManager serviceManager)
        {
            ServiceManager = serviceManager;
        }

        public IOltRuleServiceManager ServiceManager { get; }
    }
}