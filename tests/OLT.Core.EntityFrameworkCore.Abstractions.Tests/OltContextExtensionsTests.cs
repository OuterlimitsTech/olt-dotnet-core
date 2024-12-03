using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltContextExtensionsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltContextExtensionsTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltContextExtensionDb")
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
    public async Task GetTableName_ShouldReturnCorrectTableName()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        var tableName = context.GetTableName<TestEntity>();

        // Assert
        Assert.Equal("TestEntities", tableName);
    }

    [Fact]
    public async Task GetColumns_ShouldReturnCorrectColumns()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        var columns = context.GetColumns<TestEntity>().ToList();

        // Assert
        Assert.Contains(columns, c => c.Name == "Id" && c.Type == "integer");
        Assert.Contains(columns, c => c.Name == "Name" && c.Type == "text");
    }

    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext : DbContext, IOltDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        public string DefaultAnonymousUser => "Anonymous";
        public string AuditUser => "AuditUser";
        public bool ApplyGlobalDeleteFilter => true;

        //public new DatabaseFacade Database => base.Database;

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
