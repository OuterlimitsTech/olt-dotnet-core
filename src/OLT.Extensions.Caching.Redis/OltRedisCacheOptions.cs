﻿using System;

namespace OLT.Core
{
    [Obsolete("Removing in 10.x, Move to FusionCache")]
    public class OltRedisCacheOptions : OltCacheOptions
    {
        public OltRedisCacheOptions() { }

        public OltRedisCacheOptions(string cacheKeyPrefix, TimeSpan defaultAbsoluteExpiration)
        {
            CacheKeyPrefix = cacheKeyPrefix;
            DefaultAbsoluteExpiration = defaultAbsoluteExpiration;
        }

        /// <summary>
        /// Used for distributed cache to prevent collisions with other applications.
        /// </summary>
        public string CacheKeyPrefix { get; set; } = Guid.NewGuid().ToString();

        
        public new OltRedisCacheOptions Value
        {
            get { return this; }
        }
    }
}
