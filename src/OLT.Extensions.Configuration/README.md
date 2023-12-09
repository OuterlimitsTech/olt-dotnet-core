[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## IConfiguration extensions

### Configuration

```csharp

// Examples of Connection String Parsers
services.Configure<OltConnectionConfigApiKey>("GrowthBook", options => options.Parse(configuration.GetOltConnectionString("GrowthBook")));
services.Configure<OltConnectionConfigRabbitMq>(opts => opts.Parse(Configuration.GetOltConnectionString("RabbitMq")));
services.Configure<OltConnectionConfigAmazon>(opts => opts.Parse(Configuration.GetOltConnectionString("AWS")));
services.Configure<OltConnectionConfigWcf>("AddressLookup", options => options.Parse(configuration.GetOltConnectionString("AddressLookup")));


// Example of GetOltConnectionString
// This looks for "connection-string" first, then falls back to the config.GetConnectionString()
// WHY?  When you have Azure App Config or AWS Parameter store as the last configuration provider loaded, you need a way to use a local connection string.
services
    .AddDbContext<DatabaseContext>(optionsBuilder =>
    {
        optionsBuilder
              .UseSqlServer(configuration.GetOltConnectionString(AppConstants.ConnectionStrings.DbConnectionName), opt => opt.CommandTimeout(120));

        if (System.Diagnostics.Debugger.IsAttached)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    });

```

### Example 1

```csharp

public class GrowthBookClient : IGrowthBookClient
{
    readonly HttpClient _client;
    readonly ILogger<GrowthBookClient> _logger;
    readonly OltConnectionConfigApiKey _config;

    public GrowthBookClient(HttpClient client, IOptionsSnapshot<OltConnectionConfigApiKey> options, ILogger<GrowthBookClient> logger)
    {
        _client = client;
        _logger = logger;
        _config = options.Get("GrowthBook");
    }

    public async Task<IEnumerable<FeatureFlagDto>> GetFeatureFlagsAsync(CancellationToken stoppingToken = default(CancellationToken))
    {
        if (string.IsNullOrEmpty(_config.Endpoint)) throw new Exception("Invalid GrowthBook Endpoint");

        var response = await _client.GetAsync(_config.Endpoint, stoppingToken);

        if (response.IsSuccessStatusCode)
        {
           // Do Stuff Here
        }
        else
        {
            throw new OltException($"GrowthBook Http Error: {response.StatusCode}");
        }
    }
}

```

### Example 2 (using MassTransit RabbitMq)

```csharp

services.AddMassTransit(transitConfigure =>
{
    transitConfigure.AddConsumer<QueueItemConsumer>();
    transitConfigure.AddDelayedMessageScheduler();
    transitConfigure.SetKebabCaseEndpointNameFormatter();

    transitConfigure.UsingRabbitMq((context, cfg) =>
    {
        var connectionConfig = context.GetRequiredService<IOptions<OltConnectionConfigRabbitMq>>().Value;
        if (string.IsNullOrEmpty(connectionConfig.Host)) throw new Exception("Invalid RabbitMq Connection String");

        cfg.Host(new Uri(connectionConfig.Host), h =>
        {
            h.Username(connectionConfig.Username);
            h.Password(connectionConfig.Password);
        });

        cfg.ReceiveEndpoint("queue-name-here", ep =>
        {
            ep.PrefetchCount = 32;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<QueueItemConsumer>(context);
        });

    });

});
```
