using System;


namespace OLT.Core
{
    public class OltAwsConfigurationOptions
    {

        /// <summary>
        /// System Manager Parameter Store Options
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="rootPath"></param>
        /// <param name="environmentName"></param>
        /// <param name="fallbackDefault"><see cref="FallbackDefault"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public OltAwsConfigurationOptions(OltAwsBasicProfile profile, string rootPath, string environmentName, string fallbackDefault = "Common")
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentNullException(nameof(rootPath));
            }

            if (string.IsNullOrWhiteSpace(environmentName))
            {
                throw new ArgumentNullException(nameof(environmentName));
            }

            if (string.IsNullOrWhiteSpace(fallbackDefault))
            {
                fallbackDefault = "Common";
            }

            Profile = profile;
            RootPath = rootPath;
            EnvironmentName = environmentName;
            FallbackDefault = fallbackDefault;
        }

        /// <summary>
        /// Connection and Credentials profile
        /// </summary>
        public OltAwsBasicProfile Profile { get; }

        /// <summary>
        /// Root Path of the parameter store (i.e., "/MyAppName")
        /// </summary>
        public string RootPath { get; }

        /// <summary>
        /// Hosting Environment
        /// </summary>
        public string EnvironmentName { get; }


        /// <summary>
        /// Default Common values regardless of environment.  The environment value will override the fallback values
        /// </summary>
        public string FallbackDefault { get; }

        public string BuildPathFallback()
        {
            var rootPath = RootPath.TrimStart('/').TrimEnd('/');
            var fallbackDefault = FallbackDefault.TrimStart('/').TrimEnd('/');
            return $"/{rootPath}/{fallbackDefault}/";
        }

        public string BuildPathEnvironment()
        {
            var rootPath = RootPath.TrimStart('/').TrimEnd('/');
            var environmentName = EnvironmentName.TrimStart('/').TrimEnd('/');
            return $"/{rootPath}/{environmentName}/";
        }
    }
}