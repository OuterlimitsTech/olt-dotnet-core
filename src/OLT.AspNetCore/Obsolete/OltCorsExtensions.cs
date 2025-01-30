using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    [Obsolete("Removing 9.x, provides little value")]
    public static partial class OltCorsExtensions
    {

        /// <summary>
        /// Scans provided assemblies and thier referenced assemblies looking for <seealso cref="IOltAspNetCoreCorsPolicy"/> and adds the Cors policy
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="assembliesToScan">List of Assemblies to scan</param>
        /// <param name="filter"></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        [Obsolete("Removing 9.x, provides little value")]
        public static IServiceCollection AddCors(this IServiceCollection services, List<Assembly> assembliesToScan, OltAssemblyFilter? filter = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembliesToScan);

            assembliesToScan
                .GetAllReferencedAssemblies(filter)
                .GetAllImplements<IOltAspNetCoreCorsPolicy>()
                .ToList()
                .ForEach(policy => services.AddCors(policy));

            return services;
        }


        /// <summary>
        /// Adds CORS policy
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="policy"><seealso cref="IOltAspNetCoreCorsPolicy"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        [Obsolete("Removing 9.x, provides little value")]
        public static IServiceCollection AddCors(this IServiceCollection services, IOltAspNetCoreCorsPolicy policy)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(policy);

            return policy.AddCors(services);
        }

        /// <summary>
        /// Registers middleware <seealso cref="CorsMiddlewareExtensions"/> using <seealso cref="OltAspNetHostingOptions.CorsPolicyName"/> 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="options"><seealso cref="OltAspNetHostingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        [Obsolete("Removing 9.x, provides little value")]
        public static IApplicationBuilder UseCors<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : OltAspNetHostingOptions
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(options);

            return string.IsNullOrWhiteSpace(options.CorsPolicyName) ? app : app.UseCors(options.CorsPolicyName);
        }
    }
}
