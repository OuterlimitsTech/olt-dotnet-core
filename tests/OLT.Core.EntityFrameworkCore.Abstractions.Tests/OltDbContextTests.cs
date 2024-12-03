using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Constants;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltDbContextTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    private const string InitialCatalog = "OltDbContextDb";

    public OltDbContextTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase(InitialCatalog)
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
    public async Task SaveChangesAsync_ShouldSetAuditFields()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
            .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .BuildServiceProvider();

        var auditUser = serviceProvider.GetRequiredService<IOltDbAuditUser>();
        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test" };
        context.TestEntities.Add(entity);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(auditUser.GetDbUsername(), entity.CreateUser);
        Assert.Equal(auditUser.GetDbUsername(), entity.ModifyUser);
        Assert.NotEqual(default, entity.CreateDate);
        Assert.NotEqual(default, entity.ModifyDate);

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSetAbstractFields()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
            .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .BuildServiceProvider();


        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test" };
        context.TestEntities.Add(entity);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(Guid.Empty, entity.UniqueId);
        Assert.Equal(OltCommonDefaults.SortOrder, entity.SortOrder);

        await serviceProvider.DisposeAsync();
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

    public class TestDbContext : OltDbContext<TestDbContext>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        public override string DefaultSchema => "public";
        public override bool DisableCascadeDeleteConvention => false;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => true;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestEntity>();
        }
    }

    public class TestDbAuditUser : IOltDbAuditUser
    {
        private readonly string _email = Faker.Internet.Email();

        public string? GetDbUsername()
        {
            return _email;
        }
    }

}
