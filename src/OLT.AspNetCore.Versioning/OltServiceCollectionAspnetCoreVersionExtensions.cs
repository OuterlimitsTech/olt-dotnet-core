using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OLT.Core
{
    public static class OltServiceCollectionAspnetCoreVersionExtensions
    {
        /// <summary>
        /// Adds API version as query string and defaults to 1.0 if not present
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="options"><seealso cref="OltOptionsApiVersion"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddApiVersioning(this IServiceCollection services, OltOptionsApiVersion options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services
                .AddApiVersioning(opt =>
                {
                    opt.ApiVersionReader = ApiVersionReader.Combine(options.Parameter.BuildReaders());
                    opt.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersion;
                    opt.DefaultApiVersion = options.DefaultVersion;
                    opt.ReportApiVersions = options.ReportVersions;
                })
                .AddApiExplorer(opt =>
                {
                    //The format of the version added to the route URL  
                    opt.GroupNameFormat = "'v'VVV";
                    //Tells swagger to replace the version in the controller route  
                    opt.SubstituteApiVersionInUrl = true;
                })
                .AddMvc();

            return services;
        }
    }
}