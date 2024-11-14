using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    [Obsolete("Removing 9.x, Being Removed in a future release.  Provides no value")]
    public abstract class OltHostServiceBase : OltDisposable, IOltHostService
    {
        public abstract string ResolveRelativePath(string filePath);
        public abstract string EnvironmentName { get; }
        public abstract string ApplicationName { get; }

    }
}