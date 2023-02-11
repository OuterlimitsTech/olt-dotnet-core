using Asp.Versioning;

namespace OLT.Core
{
    public class OltOptionsApiVersion 
    {

        /// <summary>
        /// Indicating whether a default version is assumed when a client does not provide a service API version.
        /// </summary>
        /// <remarks>
        /// Default true
        /// </remarks>
        public virtual bool AssumeDefaultVersion { get; set; } = true;

        /// <summary>
        /// Get/Set Default Version Number if Unspecified 
        /// </summary>
        /// <remarks>
        /// Default is <see cref="ApiVersion.Default"/>
        /// </remarks>
        public virtual ApiVersion DefaultVersion { get; set; } = ApiVersion.Default;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Default is true
        /// </remarks>
        public bool ReportVersions { get; set; } = true;

        /// <summary>
        /// API version parameter names
        /// </summary>
        public virtual OltOptionsApiVersionParameter Parameter { get; set; } = new OltOptionsApiVersionParameter();

    }
}