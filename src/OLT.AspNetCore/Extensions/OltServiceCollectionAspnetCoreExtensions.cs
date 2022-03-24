using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public static class OltServiceCollectionAspnetCoreExtensions
    {

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">Invoked after initialized</param>
        /// <returns></returns>
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, Action<IMvcBuilder> action = null)
        {
            return AddOltAspNetCore(services, new List<Assembly>(), action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <returns></returns>
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, Assembly baseAssembly, Action<IMvcBuilder> action = null)
        {
            if (baseAssembly == null)
            {
                throw new ArgumentNullException(nameof(baseAssembly));
            }
            return AddOltAspNetCore(services, new List<Assembly>() { baseAssembly }, action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssemblies">List of assemblies to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <returns></returns>
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, List<Assembly> baseAssemblies, Action<IMvcBuilder> action = null)
        {

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (baseAssemblies == null)
            {
                baseAssemblies = new List<Assembly>();
            }

            baseAssemblies.Add(Assembly.GetEntryAssembly());
            baseAssemblies.Add(Assembly.GetExecutingAssembly());

            services
                .AddCors(baseAssemblies)
                .AddApiVersioning(new OltOptionsApiVersion())
                .AddOltInjection(baseAssemblies)
                .AddSingleton<IOltHostService, OltHostAspNetCoreService>()
                .AddScoped<IOltIdentity, OltIdentityAspNetCore>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>())
                .AddHttpContextAccessor();

            var mvcBuilder = services.AddControllers();
            action?.Invoke(mvcBuilder);

            return services;
        }



        /// <summary>
        /// Adds API version as query string and defaults to 1.0 if not present
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="options"><seealso cref="OltOptionsApiVersion"/></param>
        /// <param name="setupVersioningAction"><seealso cref="ApiVersioningOptions"/></param>
        /// <param name="setupExplorerAction"><seealso cref="ApiExplorerOptions"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddApiVersioning(this IServiceCollection services, OltOptionsApiVersion options, Action<ApiVersioningOptions> setupVersioningAction = null, Action<ApiExplorerOptions> setupExplorerAction = null)
        {   
            return services
                .AddApiVersioning(opt =>
                {
                    opt.ApiVersionReader = ApiVersionReader.Combine(options.Parameter.BuildReaders());
                    opt.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersion;
                    opt.DefaultApiVersion = options.DefaultVersion;
                    opt.ReportApiVersions = options.ReportVersions;
                    setupVersioningAction?.Invoke(opt);
                })
                .AddVersionedApiExplorer(opt =>
                {
                    //The format of the version added to the route URL  
                    opt.GroupNameFormat = "'v'VVV";
                    //Tells swagger to replace the version in the controller route  
                    opt.SubstituteApiVersionInUrl = true;
                    setupExplorerAction?.Invoke(opt);
                });
        }        
    }
}