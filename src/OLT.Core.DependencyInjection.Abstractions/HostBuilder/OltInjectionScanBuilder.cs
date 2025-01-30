using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OLT.Core;

public class OltScrutorScanBuilder
{
    private List<Assembly> _scanAssemblies = new List<Assembly>();

    /// <summary>
    /// The service collection for dependency injection.
    /// </summary>
    protected readonly IServiceCollection _services;

    public OltScrutorScanBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public virtual OltScrutorScanBuilder IncludeAssembly(Assembly assembly)
    {
        _scanAssemblies.Add(assembly);
        return this;
    }


    public virtual OltScrutorScanBuilder IncludeAssemblies(IEnumerable<Assembly> assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }


    public virtual OltScrutorScanBuilder IncludeAssemblies(params Assembly[] assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }

    /// <summary>
    /// Scans <see cref="IOltInjectableScoped"/>, <see cref="IOltInjectableSingleton"/>, and <see cref="IOltInjectableTransient"/> to associated DI
    /// </summary>
    public virtual void Scan()
    {
        ArgumentNullException.ThrowIfNull(_services);

        _services
            .Scan(sc =>
                sc.FromAssemblies(_scanAssemblies)
                    .AddClasses(classes => classes.AssignableTo<IOltInjectableScoped>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo<IOltInjectableTransient>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                    .AddClasses(classes => classes.AssignableTo<IOltInjectableSingleton>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
        
    }

}


