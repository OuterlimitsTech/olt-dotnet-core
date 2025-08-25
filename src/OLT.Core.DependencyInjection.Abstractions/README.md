[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Base OLT Core Abtractions  

### OltDisposable

_IDisposable abstract class wrapper_


## Methods

### AddServicesFromAssemblies(IServiceCollection services, Action<OltScrutorScanBuilder> action)

Scans for services implementing `IOltInjectableScoped`, `IOltInjectableSingleton`, and `IOltInjectableTransient` interfaces and registers them with the provided `IServiceCollection`.

#### Parameters
- `services` (`IServiceCollection`): The service collection to add the services to.
- `action` (`Action<OltScrutorScanBuilder>`): An action to configure the `OltScrutorScanBuilder`.

#### Returns
- `IServiceCollection`: The service collection with the added services.

### AddServicesFromAssemblies<TBuilder>(TBuilder builder, Action<OltScrutorScanBuilder> action) where TBuilder : IOltHostBuilder

Scans assemblies and registers services with the specified `IOltHostBuilder`.

#### Parameters
- `builder` (`TBuilder`): The host builder to add the services to.
- `action` (`Action<OltScrutorScanBuilder>`): An action to configure the `OltScrutorScanBuilder`.

#### Returns
- `TBuilder`: The host builder with the added services.

## Usage

To use the `OltDependencyInjectionExtensions` class, you need to call the `AddServicesFromAssemblies` method on your `IServiceCollection` or `IOltHostBuilder` instance, passing in an action to configure the `OltScrutorScanBuilder`.

### Example
```csharp

using Microsoft.Extensions.DependencyInjection; 
using OLT.Core;

public class Startup 
{ 
    public void ConfigureServices(IServiceCollection services) 
    { 
        services.AddServicesFromAssemblies(scan => scan.IncludeAssembly(typeof(SomeTypeInYourAssembly).Assembly)); 
    } 
}

```

In this example, the `AddServicesFromAssemblies` method is used to scan and register services from the specified assembly.

## Remarks

- The `OltDependencyInjectionExtensions` class relies on the Scutor library to perform the scanning and registration of services.
- The `OltScrutorScanBuilder` class is used to configure the scanning process, including specifying which assemblies to scan.

For more information on the Scutor library, refer to the [Scutor documentation](https://github.com/khellang/Scrutor).


### Example Using OltAssemblyScanBuilder

```csharp
using OLT.Core;

var assemblies = new OltAssemblyScanBuilder()
    .IncludeFilter("OLT.", "MyApp.")
    .IncludeAssembly(typeof(LocalServiceCollectionExtenstions).Assembly, typeof(AnotherClassName).Assembly, typeof(IAppInterfaceHere).Assembly)
    .ExcludeMicrosoft()
    .ExcludeAutomapper()
    .DeepScan()
    .Build();

services.AddServicesFromAssemblies(builder => builder.IncludeAssemblies(assemblies))
        .AddAppCors()
        .AddScoped<IAppIdentity, AppIdentity>()
        .AddScoped<IOltIdentity>(x => x.GetRequiredService<IAppIdentity>())
        .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<IAppIdentity>())
        .AddHttpContextAccessor();

```

For more information on the OltAssemblyScanBuilder library, refer to the [Documentation](https://github.com/OuterlimitsTech/olt-dotnet-utility-libraries/blob/b800cc75911f83332a98e07b5224c86d1ec1066b/src/OLT.Utility.AssemblyScanner/README.md).
