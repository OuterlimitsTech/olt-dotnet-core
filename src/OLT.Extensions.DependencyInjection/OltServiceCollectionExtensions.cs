using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public static class OltServiceCollectionExtensions
    {

        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// Adds <see cref="IOltDbAuditUser"/> to resolve to <see cref="IOltIdentity"/> as scoped
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services)
        {
            return AddOltInjection(services, new List<Assembly>());
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// Adds <see cref="IOltDbAuditUser"/> to resolve to <see cref="IOltIdentity"/> as scoped
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services, Assembly baseAssembly)
        {
            if (baseAssembly == null)
            {
                throw new ArgumentNullException(nameof(baseAssembly));
            }

            return AddOltInjection(services, new List<Assembly>() { baseAssembly });
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// Adds <see cref="IOltDbAuditUser"/> to resolve to <see cref="IOltIdentity"/> as scoped
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssemblies">List of Assemblies To Scan for interfaces</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services, List<Assembly> baseAssemblies)
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
            var assembliesToScan = baseAssemblies.GetAllReferencedAssemblies();

            services
                .Scan(sc =>
                    sc.FromAssemblies(assembliesToScan)
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableScoped>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableTransient>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableSingleton>())
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime());

            return services.AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IOltIdentity>());
        }
    }
}