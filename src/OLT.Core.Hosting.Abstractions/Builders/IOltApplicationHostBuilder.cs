using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OLT.Core
{
    public interface IOltApplicationHostBuilder : IOltHostBuilder
    {
        /// <summary>
        /// Gets the set of key/value configuration properties.
        /// </summary>
        /// <remarks>
        /// This can be mutated by adding more configuration sources, which will update its current view.
        /// </remarks>
        IConfigurationManager Configuration { get; }

        /// <summary>
        /// Gets the information about the hosting environment an application is running in.
        /// </summary>
        IHostEnvironment Environment { get; }

        /// <summary>
        /// Gets a collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        ILoggingBuilder Logging { get; }

        /// <summary>
        /// Allows enabling metrics and directing their output.
        /// </summary>
        IMetricsBuilder Metrics { get; }



        void AddConfiguration();
        void AddLoggingConfiguration();
        void AddServices();
    }
}