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
        [Obsolete("Use Scan")]
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
        [Obsolete("Use Scan")]
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
        [Obsolete("Use Scan")]
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


            return Scan(services, assembliesToScan);
        }


        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <remarks>
        /// See <a href="https://github.com/OuterlimitsTech/olt-dotnet-utility-libraries/blob/e77797d1c8a783fe7fda49968c5a45bf59add7d8/src/OLT.Utility.AssemblyScanner/README.md">documentation</a> for more details.
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection Scan(this IServiceCollection services, Action<OltAssemblyScanBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = new OltAssemblyScanBuilder();
            action(builder);
            var assemblies = builder.Build();

            return Scan(services, assemblies);
        }

        /// <summary>
        /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection Scan(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            ArgumentNullException.ThrowIfNull(services);

            services
                .Scan(sc =>
                    sc.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableScoped>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableTransient>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo<IOltInjectableSingleton>())
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime());

            return services;
        }

    }
}