using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public static class OltServiceCollectionAutoMapperExtensions
    {
        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services)
        {
            return AddOltInjectionAutoMapper(services, new List<Assembly>(), null);
        }

        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, Assembly includeAssemblyScan)
        {
            if (includeAssemblyScan == null)
            {
                throw new ArgumentNullException(nameof(includeAssemblyScan));
            }

            return AddOltInjectionAutoMapper(services, new List<Assembly> { includeAssemblyScan }, null);
        }

        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services,  List<Assembly> includeAssembliesScan)
        {
            return AddOltInjectionAutoMapper(services, includeAssembliesScan, null);
        }

        public static IServiceCollection AddOltInjectionAutoMapper(this IServiceCollection services, List<Assembly> includeAssembliesScan, Action<IMapperConfigurationExpression> configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (includeAssembliesScan == null)
            {
                includeAssembliesScan = new List<Assembly>();
            }

            var assembliesToScan = new List<Assembly>
            {
                Assembly.GetEntryAssembly(),
                Assembly.GetExecutingAssembly()
            };

            assembliesToScan.AddRange(includeAssembliesScan);

            services.AddSingleton<IOltAdapterResolver, OltAdapterResolverAutoMapper>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
                configAction?.Invoke(cfg);
            }, assembliesToScan.GetAllReferencedAssemblies(), serviceLifetime);
            return services;
        }

        
    }
}
