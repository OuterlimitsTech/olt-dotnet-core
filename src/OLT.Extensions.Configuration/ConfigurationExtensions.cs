using System;
using Microsoft.Extensions.Configuration;

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
        public static string GetOltConnectionString(this IConfiguration config, string name)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return config.GetValue<string>($"connection-strings:{name}") ??
                   config.GetConnectionString(name);
        }
    }
}