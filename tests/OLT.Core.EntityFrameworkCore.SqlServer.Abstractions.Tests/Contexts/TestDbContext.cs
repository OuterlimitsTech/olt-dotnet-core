using Microsoft.EntityFrameworkCore;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts.Entities;
using Testcontainers.MsSql;

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts;

public class TestDbContext : DbContext
{
    public TestDbContext() : this(new DbContextOptionsBuilder<TestDbContext>().UseSqlServer("server=.;database=testDb;trusted_connection=true;").Options)
    {

    }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.SetIdentityColumns(OltSqlModelBuilderExtensionsTests.Identity_Seed, OltSqlModelBuilderExtensionsTests.Identity_Increment);
    }
}
