using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OLT.Constants;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltEntityEntryExtensionsTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltEntityEntryExtensionsTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltEntityEntryExtensionsDb")
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
    public async Task SetAuditFields_ShouldSetAuditFields()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test" };
        context.TestEntities.Add(entity);
        var entry = context.Entry(entity);

        // Act
        entry.SetAuditFields("AuditUser");
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal("AuditUser", entity.CreateUser);
        Assert.Equal("AuditUser", entity.ModifyUser);
        Assert.NotEqual(default, entity.CreateDate);
        Assert.NotEqual(default, entity.ModifyDate);
    }

    [Fact]
    public async Task SetAbstractFields_ShouldSetAbstractFields()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test" };
        context.TestEntities.Add(entity);
        var entry = context.Entry(entity);

        // Act
        entry.SetAbstractFields();
        await context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(Guid.Empty, entity.UniqueId);
        Assert.Equal(OltCommonDefaults.SortOrder, entity.SortOrder);
    }

    public class TestEntity : IOltEntityAudit, IOltEntityUniqueId, IOltEntitySortable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string CreateUser { get; set; } = "Bogus";
        public DateTimeOffset CreateDate { get; set; }
        public string? ModifyUser { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public Guid UniqueId { get; set; }
        public short SortOrder { get; set; }
    }

    public class TestDbContext : DbContext, IOltDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        public string DefaultAnonymousUser => "Anonymous";
        public string AuditUser => "AuditUser";
        public bool ApplyGlobalDeleteFilter => true;

        public new DatabaseFacade Database => base.Database;

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
