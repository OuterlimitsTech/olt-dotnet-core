﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

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
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, Action<IMvcBuilder>? action = null)
        {
            return AddOltAspNetCore(services, new OltInjectionAssemblyFilter(), action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">Invoked after initialized</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns></returns>
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, OltInjectionAssemblyFilter filter, Action<IMvcBuilder>? action = null)
        {
            return AddOltAspNetCore(services, new List<Assembly>(), filter, action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <returns></returns>
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, Assembly baseAssembly, Action<IMvcBuilder>? action = null)
        {
            return AddOltAspNetCore(services, baseAssembly, new OltInjectionAssemblyFilter(), action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns></returns>
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, Assembly baseAssembly, OltInjectionAssemblyFilter filter, Action<IMvcBuilder>? action = null)
        {
            ArgumentNullException.ThrowIfNull(baseAssembly);
            return AddOltAspNetCore(services, new List<Assembly>() { baseAssembly }, filter, action);
        }

        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssemblies">List of assemblies to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <returns></returns>
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, List<Assembly> baseAssemblies, Action<IMvcBuilder>? action = null)
        {
            return AddOltAspNetCore(services, baseAssemblies, new OltInjectionAssemblyFilter(), action);
        }


        /// <summary>
        /// Build Default AspNetCore Service and configures Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseAssemblies">List of assemblies to include in scan for interfaces</param>
        /// <param name="action">Invoked after initialized</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns></returns>
        [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
        public static IServiceCollection AddOltAspNetCore(this IServiceCollection services, List<Assembly> baseAssemblies, OltInjectionAssemblyFilter filter, Action<IMvcBuilder>? action = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            if (baseAssemblies == null)
            {
                baseAssemblies = new List<Assembly>();
            }

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                baseAssemblies.Add(entryAssembly);
            }            
            baseAssemblies.Add(Assembly.GetExecutingAssembly());

            services
                .AddCors(baseAssemblies, filter)    
                .AddOltInjection(baseAssemblies, filter)
                .AddSingleton<IOltHostService, OltHostAspNetCoreService>()
                .AddScoped<IOltIdentity, OltIdentityAspNetCore>()
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>())
                .AddHttpContextAccessor();

            var mvcBuilder = services.AddControllers();
            action?.Invoke(mvcBuilder);

            return services;
        }
          
    }
}