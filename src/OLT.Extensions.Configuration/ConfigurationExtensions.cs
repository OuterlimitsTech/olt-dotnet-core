using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{

    /// <summary>
    /// Extends <see cref="IConfiguration"/>.
    /// </summary>
    public static class OltConfigurationExtensions
    {

        /// <summary>
        /// Looks for connection string connection-strings:{name} using
        /// <seealso cref="ConfigurationBinder.GetValue(IConfiguration, Type, string)"/> 
        /// then falls back to
        /// <seealso cref="ConfigurationExtensions.GetConnectionString(IConfiguration, string)"/>
        /// </summary>
        /// <param name="config"><see cref="IConfiguration"/></param>
        /// <param name="name">Connection String Key Name</param>
        /// <returns>Connection String or <see langword="null"/></returns>
        [Obsolete("Removing in 10.x, this extension provides no value now")]
        public static string? GetOltConnectionString(this IConfiguration config, string name)
        {

            ArgumentNullException.ThrowIfNull(config);
            ArgumentNullException.ThrowIfNull(name);

            return config.GetValue<string>($"connection-strings:{name}") ??
                   config.GetConnectionString(name);
        }

       

    }
}