using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltEntityTransactionExtensionsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltEntityTransactionExtensionsTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltEntityTransactionDb")
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
    public async Task UsingDbTransactionAsync_ShouldCommitTransaction()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        await context.Database.UsingDbTransactionAsync(async () =>
        {
            context.TestEntities.Add(new TestEntity { Name = "Test" });
            await context.SaveChangesAsync();
        });

        // Assert
        var entity = await context.TestEntities.FirstOrDefaultAsync(e => e.Name == "Test");
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task UsingDbTransactionAsync_ShouldRollbackTransactionOnException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await context.Database.UsingDbTransactionAsync(async () =>
            {
                context.TestEntities.Add(new TestEntity { Name = "Test" });
                await context.SaveChangesAsync();
                throw new InvalidOperationException("Test exception");
            });
        });

        var entity = await context.TestEntities.FirstOrDefaultAsync(e => e.Name == "Test");
        Assert.Null(entity);
    }

    public class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
