using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        Assert.Equal("public.TestEntity", tableName);
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
        Assert.Contains(columns, c => c.Name == "TestEntityId" && c.Type == "integer");
        Assert.Contains(columns, c => c.Name == "Name" && c.Type == "character varying(20)");
    }

    [Fact]
    public async Task ProcessException_LogsExceptionEntries()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = new string('a', 300) }; // Exceeding typical varchar length
        context.TestEntities.Add(entity);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AggregateException>(() => context.SaveChangesAsync());

        Assert.NotEmpty(exception.InnerExceptions);
        Assert.Equal(5, exception.InnerExceptions.Count);
    }

    [Fact]
    public async Task InitializeQueryable_WithQueryFilter()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var list = new List<TestEntity>
        {
            new TestEntity { Name = "Test1" },
            new TestEntity { Name = "Test2", DeletedOn = DateTimeOffset.UtcNow },
            new TestEntity { Name = "Test3" },
            new TestEntity { Name = "Test4" },
            new TestEntity { Name = "Test5", DeletedOn = DateTimeOffset.UtcNow },
            new TestEntity { Name = "Test6" },
        };

        await context.TestEntities.AddRangeAsync(list);
        await context.SaveChangesAsync();

        OltContextExtensions.InitializeQueryable<TestEntity>(context).Should().BeEquivalentTo(list);
        OltContextExtensions.InitializeQueryable<TestEntity>(context, false).Should().BeEquivalentTo(list.Where(p => p.DeletedOn == null));
        Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<TestEntity>(null!));
        Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<TestEntity>(null!, true));
        Assert.Throws<ArgumentNullException>("context", () => OltContextExtensions.InitializeQueryable<TestEntity>(null!, false));     
    }

   

    [Table(nameof(TestEntity))]
    public class TestEntity : IOltEntityId, IOltEntityDeletable
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string? Name { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class TestDbContext : OltDbContext<TestDbContext>, IOltDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        public override string DefaultSchema => "public";
        public override string DefaultAnonymousUser => "Anonymous";
        public override string AuditUser => "AuditUser";
        public override bool ApplyGlobalDeleteFilter => true;
        public override bool DisableCascadeDeleteConvention => false;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.NVarchar;

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
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
