[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

# Contains OLT Logging standards for Serilog

- ### _How To:_ Configure Services to include Payload logging middleware and user session middleware

```csharp
services.AddOltSerilog(configOptions => configOptions.ShowExceptionDetails = AppSettings.Hosting.ShowExceptionDetails);
```

- ### _How To:_ Log a OltNgxLoggerMessageJson from a controller

```csharp
[AllowAnonymous]
[HttpPost, Route("")]
public ActionResult<string> Log([FromBody] OLT.Logging.Serilog.OltNgxLoggerMessageJson message)
{
    Serilog.Log.Logger.Write(message);
    return Ok("Received");
}
```
