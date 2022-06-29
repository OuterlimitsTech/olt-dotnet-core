using Microsoft.Extensions.Options;
using System;

namespace OLT.Core
{
    public class OltCacheOptions : IOptions<OltCacheOptions>
    {
        /// <summary>
        /// Default value for cache entry expiration
        /// </summary>
        public TimeSpan? DefaultAbsoluteExpiration { get; set; }

        /// <summary>
        /// Used for Cache Segmentation and required for providers like Redis
        /// </summary>
        public string KeyPrefix { get; set; }

        public OltCacheOptions Value
        {
            get { return this; }
        }
    }
}