namespace OLT.Core
{
    public class OltOptionsAzureConfig
    {
        public OltOptionsAzureConfig(string keyPrefix, string environmentName)
        {
            KeyPrefix = keyPrefix;
            EnvironmentName = environmentName;
        }
        //string keyPrefix, string environmentName, string refreshKey

        /// <summary>
        /// config prefix (i.e., AppNameHere:)
        /// </summary>
        public virtual string KeyPrefix { get; set; }


        /// <summary>
        /// Envioronment Name
        /// </summary>
        public virtual string EnvironmentName { get; set; }
    }
}
