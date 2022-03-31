[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## IServiceCollection extensions using Scutor to build DI

Uses [Scutor](https://www.nuget.org/packages/Scrutor/) to scan all associated libraries looking for IOltInjectableScoped, IOltInjectableTransient, IOltInjectableSingleton

The scan for referenced assemblies uses an extension within the [OLT.Extensions.General](https://www.nuget.org/packages/OLT.Extensions.General/) call OltSystemReflectionExtensions.GetAllReferencedAssemblies()

The default scan list automatically includes the assemblies below:

- Assembly.GetEntryAssembly()
- Assembly.GetExecutingAssembly()

# Usage

## Uses only Assembly.GetEntryAssembly() and Assembly.GetExecutingAssembly() as a basis to start scan

```csharp
services.AddOltInjection();
```

---

## Preferred Method

```csharp
services.AddOltInjection(this.GetType().Assembly);  //Adds assembly to scan list
```

---

## Specify a list of Assembies

```csharp
var assembliesToScan = new List<Assembly>
{
    Assembly.GetEntryAssembly(),
    Assembly.GetExecutingAssembly()
};

assembliesToScan.Add(this.GetType().Assembly);
assembliesToScan.Add(Assembly.GetAssembly(typeof(MyClassHere)));

assembliesToScan.AddRange(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "OLT.App.*.dll").Select(assembly => Assembly.Load(AssemblyName.GetAssemblyName(assembly))));
assembliesToScan.AddRange(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "MyApp*.dll").Select(assembly => Assembly.Load(AssemblyName.GetAssemblyName(assembly))));

services.AddOltInjection(assembliesToScan);  //Adds list of assemblies to scan list
```
