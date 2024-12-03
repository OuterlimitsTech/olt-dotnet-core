using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltEfCoreQueryableExtensionsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltEfCoreQueryableExtensionsTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltEfCoreQueryableDb")
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
    public async Task ToPagedAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        context.TestEntities.AddRange(
            new TestEntity { Name = "Test1" },
            new TestEntity { Name = "Test2" },
            new TestEntity { Name = "Test3" }
        );
        await context.SaveChangesAsync();

        var pagingParams = new OltPagingParams { Page = 1, Size = 2 };

        // Act
        var pagedResult = await context.TestEntities.ToPagedAsync(pagingParams);

        // Assert
        Assert.Equal(1, pagedResult.Page);
        Assert.Equal(2, pagedResult.Size);
        Assert.Equal(3, pagedResult.Count);
        Assert.Equal(2, pagedResult.Data.Count());
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
