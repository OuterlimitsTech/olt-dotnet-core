using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Models.Adapters;
using OLT.EF.Core.Services.Tests.Lib;
using OLT.EF.Core.Services.Tests.Lib.Abstract;
using OLT.EF.Core.Services.Tests.Lib.Repos;
using OLT.EF.Core.Services.Tests.Lib.Services;
using Testcontainers.PostgreSql;

namespace OLT.EF.Core.Services.Tests.DataAdapter;

public abstract class BaseUnitTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    protected BaseUnitTests()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("ContextServiceTests")
            .WithPassword("yourStrong(!)Password")
            .Build();
    }

    private DbContextOptions<TestDbContext> GetContextOptions()
    {
        return new DbContextOptionsBuilder<TestDbContext>().UseNpgsql(_dbContainer.GetConnectionString()).Options;
    }


    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using (var context = new TestDbContext(GetContextOptions()))
        {
            await context.Database.EnsureCreatedAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();

        services
            //.AddLogging(config => config.AddConsole())
        //    .AddAutoMapper(this.GetType().Assembly)
            .AddDbContextPool<TestDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();

            });


        services
            .AddScoped<IOltServiceManager, OltEfCoreServiceManager>()
            .AddScoped<IContextRepo, ContextRepo>()
            .AddScoped<IPersonService, PersonService>()
            .AddScoped<IPersonUniqueIdService, PersonUniqueIdService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IOltDbAuditUser, DbAuditUser>();

        services.AddOltAdapters(builder =>
        {
            builder.AssemblyScanner.IncludeFilter("OLT.");
            builder.AssemblyScanner.IncludeAssembly(this.GetType().Assembly, typeof(UserDtoAdapter).Assembly);
        });

        //services.AddOltAutoMapper(builder =>
        //{
        //    builder.AddMaps(typeof(UserModelAdapter).Assembly);
        //});

        return services.BuildServiceProvider();
    }

}
