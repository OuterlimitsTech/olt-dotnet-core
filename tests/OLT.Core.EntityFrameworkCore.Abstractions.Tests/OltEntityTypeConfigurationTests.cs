using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OLT.Constants;
using System.ComponentModel;
using Testcontainers.PostgreSql;

namespace OLT.Core.EntityFrameworkCore.Abstractions.Tests;

public class OltEntityTypeConfigurationTests : IAsyncLifetime
{
    private const string Guid1 = "3BE30BD7-9E69-4D54-934B-3F15D83FC774";

    private readonly PostgreSqlContainer _postgreSqlContainer;

    public OltEntityTypeConfigurationTests()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OltEntityTypeConfigurationDb")
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
    public async Task Configure_ShouldSeedData()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        var entities = await context.TestEntities.ToListAsync();

        var guid = new Guid(Guid1);

        // Assert
        Assert.Equal(3, entities.Count);
        Assert.Contains(entities, e => e.Id == 1 && e.Name == "First" && e.SortOrder == 10 && e.UniqueId == guid);
        Assert.Contains(entities, e => e.Id == 2 && e.Name == "Second" && e.SortOrder == 20);
        Assert.Contains(entities, e => e.Id == 3 && e.Name == "Third" && e.Code == "3rd" && e.SortOrder == OltCommonDefaults.SortOrder);
        Assert.Equal(3, entities.Count(p => p.CreateDate == OltEFCoreConstants.DefaultSeedCreateDate));
        Assert.Equal(3, entities.Count(p => p.CreateUser == OltEFCoreConstants.DefaultSeedUsername));
        Assert.Equal(3, entities.Count(p => p.UniqueId != Guid.Empty));
        Assert.Equal(3, entities.Count(p => p.ModifyDate == null));
        Assert.Equal(3, entities.Count(p => p.ModifyUser == null));
    }

    public class TestEntity : IOltEntity, IOltEntityId, IOltEntityCodeValue, IOltEntitySortable, IOltEntityAudit, IOltEntityUniqueId
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = default!;
        public short SortOrder { get; set; }
        public string CreateUser { get; set; } = "Bogus";
        public DateTimeOffset CreateDate { get; set; }
        public string? ModifyUser { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public Guid UniqueId { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TestEntityTypeConfiguration());
        }
    }

    public class TestEntityTypeConfiguration : OltEntityTypeConfiguration<TestEntity, TestEnum>
    {
        public override void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            base.Configure(builder);
        }
    }

    public enum TestEnum
    {
        [Description("First")]
        [SortOrder(10)]
        [UniqueId(Guid1)]
        First = 1,
        [Description("Second")]
        [SortOrder(20)]
        Second = 2,
        [Description("Third")]
        [Code("3rd")]
        Third = 3
    }
}
