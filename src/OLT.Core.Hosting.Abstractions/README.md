[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Hosting Abstractions that extends Microsoft IHost


```csharp

public interface IOltHostService : IOltInjectableSingleton
{
        string ResolveRelativePath(string filePath);
        string EnvironmentName { get; }
        string ApplicationName { get; }
}


//Note: add to DI
services
    .AddSingleton<IOltHostService, MyHostService>();

```
