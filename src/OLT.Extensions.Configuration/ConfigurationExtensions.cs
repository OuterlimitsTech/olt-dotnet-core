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
        public static string? GetOltConnectionString(this IConfiguration config, string name)
        {

            ArgumentNullException.ThrowIfNull(config);
            ArgumentNullException.ThrowIfNull(name);

            return config.GetValue<string>($"connection-strings:{name}") ??
                   config.GetConnectionString(name);
        }

        internal static int ToPort(this string? self, int defaultValue)
        {
            if (!int.TryParse(self, out var value))
                return defaultValue;
            return value;
        }

        internal static bool? ToBoolean(this string? self, bool? defaultValue)
        {
            if (bool.TryParse(self, out var val))
            {
                return val;
            }

            return defaultValue;
        }

    }
}