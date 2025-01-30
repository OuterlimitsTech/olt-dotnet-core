using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace OLT.Core
{
    public abstract class OltApplicationHostBuilder<THostBuilder> : IOltApplicationHostBuilder
        where THostBuilder : class, IHostApplicationBuilder
    {

        protected OltApplicationHostBuilder([NotNull] THostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            Builder = builder;
            Initialize();
        }

        /// <summary>
        /// Called from the constructor
        /// </summary>
        /// <remarks>
        /// Calls methods in this order
        /// <list type="bullet">
        /// <item>
        /// <description>Calls <see cref="AddConfiguration"/></description>
        /// </item>
        /// <item>
        /// <description>Calls <see cref="AddLoggingConfiguration"/></description>
        /// </item>
        /// <item>
        /// <description>Calls <see cref="AddServices"/></description>
        /// </item>    
        /// </list>
        /// </remarks>
        protected virtual void Initialize()
        {
            this.AddConfiguration();
            this.AddLoggingConfiguration();
            this.AddServices();
        }

        public abstract void AddConfiguration();
        public abstract void AddLoggingConfiguration();
        public abstract void AddServices();        
        public virtual THostBuilder Builder { get; }
        public virtual IServiceCollection Services => Builder.Services;
        public virtual IConfigurationManager Configuration => Builder.Configuration;
        public virtual IHostEnvironment Environment => Builder.Environment;
        public virtual IMetricsBuilder Metrics => Builder.Metrics;
        public virtual ILoggingBuilder Logging => Builder.Logging;
    }
}