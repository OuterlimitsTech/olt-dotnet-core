[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core) [![Nuget](https://img.shields.io/nuget/v/OLT.Core.Common)](https://www.nuget.org/packages/OLT.Core.Common)

## Contains common assets for OLT Library Packages

| Utility/Item/Object | Description                                        | Comments |
| ------------------- | -------------------------------------------------- | -------- |
| OltDisposable       | Implements standard abstract class for IDisposable |          |

### OltDisposable Usage

```csharp
public class TestDisposable : OltDisposable
{
  ...
}
```
