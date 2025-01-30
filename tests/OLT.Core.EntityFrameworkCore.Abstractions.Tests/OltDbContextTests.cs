using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Constants;
using System.ComponentModel.DataAnnotations;
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

    [Fact]
    public async Task SoftDelete_ShouldExcludeFromResults()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
            .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .BuildServiceProvider();


        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test", DeletedOn = DateTimeOffset.UtcNow, DeletedBy = Faker.Name.FullName() };
        context.TestEntities.Add(entity);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Null(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task SoftDelete_ShoulReturnResults()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
            .AddDbContext<TestDbContextAlt>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .BuildServiceProvider();


        var context = serviceProvider.GetRequiredService<TestDbContextAlt>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test", DeletedOn = DateTimeOffset.UtcNow, DeletedBy = Faker.Name.FullName() };
        context.TestEntities.Add(entity);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.NotNull(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Context_OverflowException()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = Faker.Lorem.Sentence(50) };
        await context.TestEntities.AddAsync(entity);
        await Assert.ThrowsAsync<AggregateException>(() => context.SaveChangesAsync());

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Context_ThrowDbUpdateException()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = null! };
        await context.TestEntities.AddAsync(entity);
        await Assert.ThrowsAsync<AggregateException>(() => context.SaveChangesAsync());

        await serviceProvider.DisposeAsync();
    }


    [Fact]
    public async Task Context_HardDelete_ShouldBeNull()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity { Name = "Test" };
        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.NotNull(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        context.TestEntities.Remove(entity);
        await context.SaveChangesAsync();

        Assert.Null(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Context_ChildHardDelete_ShouldThrowException()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var entity = new TestEntity 
        { 
            Name = "Test",
            Children = new List<TestChildEntity>
            {
                new TestChildEntity { Value = "Child"}
            }
        };

        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.NotNull(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        Assert.Throws<InvalidOperationException>(() => context.TestEntities.Remove(entity));

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Context_ChildHardCascadeDelete_ShouldDelete()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContextAlt>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContextAlt>();
        await context.Database.EnsureCreatedAsync();

        var childEntity1 = new TestChildEntity { Value = "Child1" };
        var childEntity2 = new TestChildEntity { Value = "Child2" };
        var entity = new TestEntity
        {
            Name = "Test",
            Children = new List<TestChildEntity>
            {
                childEntity1,
                childEntity2
            }
        };

        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.NotNull(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));

        context.TestEntities.Remove(entity);
        await context.SaveChangesAsync();
        Assert.Null(await context.TestEntities.FirstOrDefaultAsync(p => p.Id == entity.Id));
        Assert.Null(await context.TestChildEntities.FirstOrDefaultAsync(p => p.Id == childEntity1.Id));
        Assert.Null(await context.TestChildEntities.FirstOrDefaultAsync(p => p.Id == childEntity2.Id));

        await serviceProvider.DisposeAsync();
    }

    [Fact]
    public async Task Context_EmptyString_ShouldBeNull()
    {
        var serviceProvider = new ServiceCollection()
           .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
           .AddDbContext<TestDbContext>(options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
           .BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<TestDbContext>();
        await context.Database.EnsureCreatedAsync();

        var childEntity1 = new TestChildEntity { Value = "Child1", Value2 = "" };
        var childEntity2 = new TestChildEntity { Value = "Child2", Value2 = " " };
        var childEntity3 = new TestChildEntity { Value = "Child2", Value2 = "Value3" };
        var entity = new TestEntity
        {
            Name = "Test",
            Children = new List<TestChildEntity>
            {
                childEntity1,
                childEntity2,
                childEntity3
            }
        };

        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.NotNull(context.TestChildEntities.First(p => p.Id == childEntity1.Id));
        Assert.Null(context.TestChildEntities.First(p => p.Id == childEntity1.Id)?.Value2);
        
        Assert.NotNull(context.TestChildEntities.First(p => p.Id == childEntity2.Id));
        Assert.Null(context.TestChildEntities.First(p => p.Id == childEntity2.Id)?.Value2);

        Assert.NotNull(context.TestChildEntities.First(p => p.Id == childEntity3.Id));
        Assert.NotNull(context.TestChildEntities.First(p => p.Id == childEntity3.Id)?.Value2);

        await serviceProvider.DisposeAsync();
    }

   

    #region [ Entities ]

    public class TestEntity : IOltEntityId, IOltEntityAudit, IOltEntityUniqueId, IOltEntitySortable, IOltEntityDeletable
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; } = default!;
        public string CreateUser { get; set; } = "Bogus";
        public DateTimeOffset CreateDate { get; set; }
        public string? ModifyUser { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public Guid UniqueId { get; set; }
        public short SortOrder { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }

        public List<TestChildEntity> Children { get; set; } = new List<TestChildEntity>();
    }

    public class TestChildEntity : IOltEntityId, IOltEntityDeletable
    {
        public int Id { get; set; }

        public int TestEntityId { get; set; }
        public TestEntity TestEntity { get; set; } = default!;

        [MaxLength(20)]
        public string Value { get; set; } = default!;

        public string? Value2 { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }

    }

    public class TestExceptionEntity : IOltEntity
    {
        [Key]
        [StringLength(50)]
        public string Key { get; set; } = default!;

        private string _title = "  ";
        public string Title
        {
            get { return _title; }
            set { throw new Exception("CheckNullableStringFields"); }  //This forces an exception from the empty string to null context processes
        }

    }

    #endregion

    #region [ Contexts ]

    /// <summary>
    /// Enables GlobalFilter and disables Cascade delete
    /// </summary>
    public class TestDbContext : OltDbContext<TestDbContext>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();
        public DbSet<TestChildEntity> TestChildEntities => Set<TestChildEntity>();
        public DbSet<TestExceptionEntity> TestExceptionEntities => Set<TestExceptionEntity>();

        public override string DefaultSchema => "public";
        public override bool DisableCascadeDeleteConvention => true;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => true;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestEntity>();
        }
    }

    /// <summary>
    /// Disables the global filters and allows Cascade delete
    /// </summary>
    public class TestDbContextAlt : OltDbContext<TestDbContextAlt>
    {
        public TestDbContextAlt(DbContextOptions<TestDbContextAlt> options) : base(options) { }

        public DbSet<TestEntity> TestEntities => Set<TestEntity>();
        public DbSet<TestChildEntity> TestChildEntities => Set<TestChildEntity>();
        public DbSet<TestExceptionEntity> TestExceptionEntities => Set<TestExceptionEntity>();

        public override string DefaultSchema => "public";
        public override bool DisableCascadeDeleteConvention => false;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.NVarchar;
        public override bool ApplyGlobalDeleteFilter => false;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestEntity>();
        }
    }

    #endregion

    public class TestDbAuditUser : IOltDbAuditUser
    {
        private readonly string _email = Faker.Internet.Email();

        public string? GetDbUsername()
        {
            return _email;
        }
    }

}
