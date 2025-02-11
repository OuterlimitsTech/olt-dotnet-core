﻿namespace OLT.Core
{
    [Obsolete("Removing in 10.x, Move to FusionCache")]
    public class OltCacheOptions 
    {
        /// <summary>
        /// Default value for cache entry expiration
        /// </summary>
        public TimeSpan DefaultAbsoluteExpiration { get; set; } = TimeSpan.FromSeconds(1);

        public OltCacheOptions Value
        {
            get { return this; }
        }
    }
}