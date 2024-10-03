namespace OLT.Core
{
    public abstract class OltCoreService : OltDisposable, IOltCoreService
    {
    }

    public abstract class OltCoreService<TServiceManager> : OltCoreService
        where TServiceManager : class, IOltServiceManager
    {
        protected OltCoreService(IOltServiceManager serviceManager)
        {
            ServiceManager = (TServiceManager)serviceManager;
        }

        protected virtual TServiceManager ServiceManager { get; }
    }


}