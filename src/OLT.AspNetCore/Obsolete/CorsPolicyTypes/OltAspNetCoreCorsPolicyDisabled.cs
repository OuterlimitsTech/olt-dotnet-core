﻿using Microsoft.Extensions.DependencyInjection;
using OLT.Constants;
using System;

namespace OLT.Core
{
    [Obsolete("Removing 9.x, provides little value")]
    public class OltAspNetCoreCorsPolicyDisabled : IOltAspNetCoreCorsPolicy   
    {
        public string PolicyName => OltAspNetDefaults.CorsPolicies.Disabled;

        /// <summary>
        /// Sets CORS policy
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection AddCors(IServiceCollection services)
        {

            return services.AddCors(o => o.AddPolicy(PolicyName, builder => {  }));
        }
    }
}