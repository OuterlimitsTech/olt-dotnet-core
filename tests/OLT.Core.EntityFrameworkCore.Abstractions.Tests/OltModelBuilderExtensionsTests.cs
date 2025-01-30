using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltModelBuilderExtensionsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltModelBuilderExtensionsTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltModelBuilderExtensionDb")
            .WithUsername("postgres")
            .WithPassword("yourStrong(!)Password")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task EntitiesOfType_ShouldApplyBuildAction()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();


        // Assert
        var entityType = context.Model.FindEntityType(typeof(TestEntity));
        var property = entityType.FindProperty("TestProperty");
        Assert.NotNull(property);
        Assert.Equal(50, property.GetMaxLength());
    }

    [Fact]
    public async Task SetSoftDeleteGlobalFilter_ShouldApplyGlobalFilter()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();


        // Assert
        var entityType = context.Model.FindEntityType(typeof(TestEntity));
        var queryFilter = entityType.GetQueryFilter();
        Assert.NotNull(queryFilter);
        Assert.Contains("DeletedOn", queryFilter.Body.ToString());
    }

    public interface ITestEntity
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public class TestEntity : ITestEntity, IOltEntityDeletable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EntitiesOfType<ITestEntity>(builder =>
            {
                builder.Property<string>("TestProperty").HasMaxLength(50);
            });

            modelBuilder.SetSoftDeleteGlobalFilter();
            modelBuilder.Entity<TestEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }

}
