using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.AspNetCore;

namespace OLT.Logging.Serilog
{

    public static class OltSerilogConstants
    {
        public static class FormatString
        {
            public const string ISO8601 = "yyyy-MM-ddTHH:mm:ss.fffZ";
        }


        public static class Properties
        {
            public const string Username = "Username";
            public const string DbUsername = "DbUsername";
            public const string EventType = "OltEventType";
            public const string Environment = "Environment";
            public const string DebuggerAttached = "DebuggerAttached";
            public const string Application = "Application";

            public static class AspNetCore
            {
                public const string AppRequestUid = "AppRequestUid";
                public const string RequestHeaders = "RequestHeaders";
                public const string ResponseHeaders = "ResponseHeaders";
                public const string RequestBody = "RequestBody";
                public const string ResponseBody = "ResponseBody";
                public const string RequestUri = "RequestUri";
            }

            public static class NgxMessage
            {
                public const string MessageAsJson = "ngx-message-json";
            }
        }



        public static class Templates
        {
            public static class AspNetCore
            {
                public const string ServerError = "{AppRequestUid}:{Message}";
                public const string Payload = "{AppRequestUid}:APP PAYLOAD LOG {RequestMethod} {RequestPath} {statusCode}";
            }

            public const string DefaultOutput =
                "[{Timestamp:HH:mm:ss} {Level:u3}] {OltEventType:x8} {Message:lj}{NewLine}{Exception}";



            public static class Email
            {
                public static string DefaultEmail => Environment.NewLine +
                                                     Environment.NewLine +
                                                     DefaultOutput +
                                                     Environment.NewLine +
                                                     Environment.NewLine;

                public const string DefaultSubject =
                    "APPLICATION {Level} on {Application} {Environment} Environment occurred at {Timestamp}";
            }

            public static class NgxMessage
            {
                public const string Template = "ngx-message: {ngx-message}";
            }
        }

    }

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
