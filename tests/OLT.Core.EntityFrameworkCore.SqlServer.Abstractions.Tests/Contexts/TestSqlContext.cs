using Microsoft.EntityFrameworkCore;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts.Entities;

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts;

public class TestSqlContext : OltSqlDbContext<TestSqlContext>
{
    public const int Identity_Seed = 10;
    public const int Identity_Increment = 5;

    public TestSqlContext() : base(new DbContextOptionsBuilder<TestSqlContext>().UseSqlServer("server=.;database=testSqlDb;trusted_connection=true;").Options)
    {
    }

    public TestSqlContext(DbContextOptions<TestSqlContext> options) : base(options)
    {
    }

    public override string DefaultSchema => "dbo";
    public override bool DisableCascadeDeleteConvention => true;
    public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
    public override bool ApplyGlobalDeleteFilter => true;
    protected override int IdentitySeed => Identity_Seed;
    protected override int IdentityIncrement => Identity_Increment;

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();


}
