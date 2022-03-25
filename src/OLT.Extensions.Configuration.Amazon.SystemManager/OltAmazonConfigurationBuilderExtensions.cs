using Microsoft.Extensions.Configuration;
using System;


namespace OLT.Core
{

    /// <summary>
    /// Extends <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class OltAmazonConfigurationBuilderExtensions
    {

        /// <summary>
        /// Configures AWS System Manager Parameter Store
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <param name="reloadAfter">Default is 10 minutes</param>
        /// <param name="optional"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IConfigurationBuilder AddSystemsManager(this IConfigurationBuilder builder, OltAwsConfigurationOptions options, TimeSpan? reloadAfter = null, bool optional = false)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }


            var aws = options.Profile.Build();
            builder.AddSystemsManager(options.BuildPathFallback(), aws, optional: optional, reloadAfter ?? TimeSpan.FromMinutes(10));
            builder.AddSystemsManager(options.BuildPathEnvironment(), aws, optional: optional, reloadAfter ?? TimeSpan.FromMinutes(10));

            return builder;
        }
    }
}