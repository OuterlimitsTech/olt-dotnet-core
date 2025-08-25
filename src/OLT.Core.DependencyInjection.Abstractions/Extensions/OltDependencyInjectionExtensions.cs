using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core;

/// <summary>
/// <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, <see cref="IOltInjectableTransient"/> using Scan Utility using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
/// </summary>
public static class OltDependencyInjectionExtensions
{
    /// <summary>
    /// Scans for <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, <see cref="IOltInjectableTransient"/> using Scan Utility using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    /// </summary>
    /// <remarks>
    /// uses Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    /// </remarks>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="action">An action to configure the OltAutoMapperBuilder.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection AddServicesFromAssemblies(this IServiceCollection services, Action<OltScrutorScanBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(services);
        var scanBuilder = new OltScrutorScanBuilder(services);
        action(scanBuilder);
        scanBuilder.Scan();
        return services;
    }


    /// <summary>
    /// Scans Assemblies to the service collection and scans the specified assemblies.
    /// </summary>
    /// <remarks>
    /// uses Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    /// </remarks>
    /// <param name="builder">The IServiceCollection to add the services to.</param>
    /// <param name="action">An action to configure the OltAutoMapperBuilder.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static TBuilder AddServicesFromAssemblies<TBuilder>(this TBuilder builder, Action<OltScrutorScanBuilder> action) where TBuilder : IOltHostBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        AddServicesFromAssemblies(builder.Services, action);
        return builder;
    }    

}
