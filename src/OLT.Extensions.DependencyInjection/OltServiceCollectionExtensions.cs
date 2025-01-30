using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;
using System.Reflection;

namespace OLT.Core
{
    public static class OltServiceCollectionExtensions
    {

        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        [Obsolete("Use AddServicesFromAssemblies")]
        public static IServiceCollection AddOltInjection(this IServiceCollection services, OltInjectionAssemblyFilter? filter = null)
        {
            return AddOltInjection(services, new List<Assembly>(), filter);
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssembly">Assembly to include in scan for interfaces</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns><param typeof="IServiceCollection"></param></returns>
        [Obsolete("Use AddServicesFromAssemblies")]
        public static IServiceCollection AddOltInjection(this IServiceCollection services, Assembly baseAssembly, OltInjectionAssemblyFilter? filter = null)
        {
            ArgumentNullException.ThrowIfNull(baseAssembly);
            return AddOltInjection(services, new List<Assembly>() { baseAssembly }, filter);
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="baseAssemblies">List of Assemblies To Scan for interfaces</param>
        /// <param name="filter">Assembly Filter</param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        [Obsolete("Use AddServicesFromAssemblies")]
        public static IServiceCollection AddOltInjection(this IServiceCollection services, List<Assembly> baseAssemblies, OltInjectionAssemblyFilter? filter = null)
        {
            ArgumentNullException.ThrowIfNull(services);

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
            var assembliesToScan = baseAssemblies.GetAllReferencedAssemblies(filter).ToList();
            filter.RemoveAllExclusions(assembliesToScan);
            return OltDependencyInjectionExtensions.AddServicesFromAssemblies(services, opt => opt.AddAssemblies(assembliesToScan));
        }



    }
}