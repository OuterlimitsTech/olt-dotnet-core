using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="filter">Assembly Filter</param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services, OltInjectionAssemblyFilter? filter = null)
        {
            return AddOltInjection(services, new List<Assembly>(), filter);
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// Adds <see cref="IOltDbAuditUser"/> to resolve to <see cref="IOltIdentity"/> as scoped
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services, Assembly baseAssembly, OltInjectionAssemblyFilter? filter = null)
        {
            if (baseAssembly == null)
            {
                throw new ArgumentNullException(nameof(baseAssembly));
            }

            return AddOltInjection(services, new List<Assembly>() { baseAssembly }, filter);
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// Adds <see cref="IOltDbAuditUser"/> to resolve to <see cref="IOltIdentity"/> as scoped
        /// </remarks>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssemblies">List of Assemblies To Scan for interfaces</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddOltInjection(this IServiceCollection services, List<Assembly> baseAssemblies, OltInjectionAssemblyFilter? filter = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (baseAssemblies == null)
            {
                baseAssemblies = new List<Assembly>();
            }

            filter = filter ?? new OltInjectionAssemblyFilter();            

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                baseAssemblies.Add(entryAssembly);
            }
            
            baseAssemblies.Add(Assembly.GetExecutingAssembly());
            var assembliesToScan = baseAssemblies.GetAllReferencedAssemblies();


            filter.ExcludeFilter.ForEach(excludeName => RemoveExclusions(assembliesToScan, excludeName));
            

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

        private static void RemoveExclusions(IEnumerable<Assembly> assemblies, string excludeName)
        {
            if (excludeName.EndsWith("*")) 
            {
                var wildCard = excludeName.Replace("*", string.Empty);
                assemblies.ToList().RemoveAll(p => p.FullName != null && p.FullName.StartsWith(wildCard, StringComparison.OrdinalIgnoreCase));
                return;
            }

            assemblies.ToList().RemoveAll(p => p.FullName != null && p.FullName.Equals(excludeName, StringComparison.OrdinalIgnoreCase));
        }
    }
}