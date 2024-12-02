using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Lib;

public class TestDbContext : OltDbContext<TestDbContext>
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {

    }

    public override string DefaultSchema => "public";
    public override bool DisableCascadeDeleteConvention => true;
    public override OltContextStringTypes DefaultStringType => OltContextStringTypes.NVarchar;
    public override bool ApplyGlobalDeleteFilter => false;

    public virtual DbSet<PersonEntity> People => Set<PersonEntity>();
    public virtual DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public virtual DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.SetIdentityColumns(OltSqlModelBuilderExtensionsTests.Identity_Seed, OltSqlModelBuilderExtensionsTests.Identity_Increment);
    }
}
