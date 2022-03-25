[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## IConfiguration extensions for AWS Parameter Store using AccessKey/Secret

###

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
        var accessKey = settings.GetValue<string>("AWS:AccessKey") ?? Environment.GetEnvironmentVariable("AWS:AccessKey");
        var secretKey = settings.GetValue<string>("AWS:SecretKey") ?? Environment.GetEnvironmentVariable("AWS:SecretKey");

        var profile = new OltAwsBasicProfile(RegionEndpoint.USEast2, accessKey, secretKey);
        builder.AddSystemsManager(new OltAwsConfigurationOptions(profile, "UnitTests", RunEnvironment, "Default"), TimeSpan.FromMinutes(10), false);

    })
```
