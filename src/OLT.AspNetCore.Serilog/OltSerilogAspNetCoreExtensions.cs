﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.AspNetCore;

namespace OLT.Logging.Serilog
{

    public static class OltSerilogAspNetCoreExtensions
    {

        /// <summary>
        /// Registers OLT assets like middleware objects used for Serilog
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddOltSerilog(this IServiceCollection services, Action<OltSerilogOptions> configOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services
                .Configure<OltSerilogOptions>(binding =>
                {
                    var serilogOptions = new OltSerilogOptions();
                    configOptions(serilogOptions);
                    binding.ShowExceptionDetails = serilogOptions.ShowExceptionDetails;
                })
                .AddScoped<OltMiddlewarePayload>()
                .AddScoped<OltMiddlewareSession>();
        }

        /// <summary>
        /// Registers middleware <seealso cref="SerilogApplicationBuilderExtensions"/>, <seealso cref="OltMiddlewareSession"/> and <seealso cref="OltMiddlewarePayload"/>
        /// </summary>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="configureOptions"><seealso cref="RequestLoggingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseOltSerilogRequestLogging(this IApplicationBuilder app, Action<RequestLoggingOptions>? configureOptions = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app
                //.UseSerilogRequestLogging(configureOptions)
                .UseMiddleware<OltMiddlewareSession>()
                .UseMiddleware<OltMiddlewarePayload>();
        }


    }


    
}
