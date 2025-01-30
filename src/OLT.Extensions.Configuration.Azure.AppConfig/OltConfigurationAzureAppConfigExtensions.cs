using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace OLT.Core
{

    public static class OltAzureAppConfigExtensions
    {

        /// <summary>
        /// Configures Connection to Azure App Config using Azure Idenity (Default is <see cref="ManagedIdentityCredential"/>)
        /// </summary>
        /// <remarks>
        /// Azure App Config endpoint example:  https://sample.azconfig.io
        /// </remarks>
        /// <param name="options"></param>
        /// <param name="endpoint">Azure App Config endpoint (i.e., https://sample.azconfig.io)</param>
        /// <param name="credential">Credential to use (Default is <see cref="ManagedIdentityCredential"/>)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AzureAppConfigurationOptions Connect(this AzureAppConfigurationOptions options, string endpoint, TokenCredential? credential = null)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(endpoint);

            credential = credential ?? new ManagedIdentityCredential();
            return options.Connect(new Uri(endpoint), credential);
        }


        /// <summary>
        /// Configures the Default Azure Configuration
        /// </summary>
        /// <param name="options"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AzureAppConfigurationOptions OltAzureConfigDefault(this AzureAppConfigurationOptions options, OltOptionsAzureConfig config)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(config);

            var keyFilter = $"{config.KeyPrefix}*";

            return options
                .TrimKeyPrefix(config.KeyPrefix)
                .Select(keyFilter, LabelFilter.Null)
                .Select(keyFilter, config.EnvironmentName);
        }
    }
}
