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

        public OltCacheOptions Value
        {
            get { return this; }
        }
    }
}