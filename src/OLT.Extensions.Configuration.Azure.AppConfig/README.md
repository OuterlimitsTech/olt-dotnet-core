[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## IConfiguration extensions for Azure App Config Service

### Example using Connection String

```csharp
 Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        var environmentName = hostContext.HostingEnvironment.EnvironmentName;
        builder
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        builder.AddUserSecrets<Program>();

        var settings = builder.Build();
        var connectionString = settings.GetValue<string>("AZURE_APP_CONFIG_CONNECTION_STRING") ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_CONNECTION_STRING");

        builder.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(connectionString)
                .OltAzureConfigDefault(new OltOptionsAzureConfig("AppKeyPrefix:", RunEnvironment)); // AppKeyPrefix: will be trimmed from the configuration
        });
    })
```

### Example using Azure Identity

https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme

```csharp
 Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        var environmentName = hostContext.HostingEnvironment.EnvironmentName;
        builder
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        builder.AddUserSecrets<Program>();

        var settings = builder.Build();

        var endpoint = settings.GetValue<string>("AZURE_APP_CONFIG_ENDPOINT") ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_ENDPOINT");

        builder.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(endpoint, new EnvironmentCredential())
                .OltAzureConfigDefault(new OltOptionsAzureConfig("AppKeyPrefix:", RunEnvironment)); // AppKeyPrefix: will be trimmed from the configuration
        });
    })
```
